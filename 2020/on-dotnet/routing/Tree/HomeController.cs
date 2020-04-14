using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tree
{
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return LocalRedirect("~/index.html");
        }

        public IActionResult Privacy()
        {
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return null;
        }

        private struct PathSegment
        {
            public int Start { get; set; }

            public int Length { get; set; }
        }

        public int GetDestination(string path, int start, int length)
        {
            // We get the raw bytes so we can be fancy
            ReadOnlySpan<char> span = path.AsSpan(start, length);
            ref char charRef = ref MemoryMarshal.GetReference(span);
            ref byte p = ref Unsafe.As<char, byte>(ref charRef);

            ulong uint64Value;
            ulong uint64LowerIndicator;
            ulong uint64UpperIndicator;

            // We know all of the "expected" lengths of strings so we can branch on lengths.
            // We can now vectorize efficiently because we're always comparing
            // same lengths.
            //
            // Depending on the length we might vectorize by 4 or 2 characters at a time.
            if (length == 16)
            {
                // Reading 4 characters = 8 bytes
                uint64Value = Unsafe.ReadUnaligned<ulong>(ref p);
                p = ref Unsafe.Add(ref p, 8);

                // Figure out if this is ASCII
                // This algorithm only handles ASCII, so we fall back to a slower
                // algorithm if necessary
                if ((uint64Value & ~0x007F007F007F007FUL) == 0)
                {
                    return -2; // NOT ASCII
                }

                // Vectorized computation of lowercase version of the string
                uint64LowerIndicator = uint64Value + (0x0080008000800080UL - 0x0041004100410041UL);
                uint64UpperIndicator = uint64Value + (0x0080008000800080UL - 0x005B005B005B005BUL);
                ulong temp1 = uint64LowerIndicator ^ uint64UpperIndicator;
                ulong temp2 = temp1 & 0x0080008000800080UL;
                ulong temp3 = (temp2) >> 2;
                uint64Value = uint64Value ^ temp3;

                // Since we know all of the strings, we can compute ahead of time what we're comparing
                // against.
                if (uint64Value == 0x0034003400340034UL)
                {
                    // Repeating this process until we've walked the entire length.
                    // Code like the above would also appear inside here.
                }

                if (uint64Value == 0x0032003200320024UL)
                {
                    // Repeating this process until we've walked the entire length.
                    // Code like the above would also appear inside here.
                }

                return -1; // No matches
            }

            return -1; // No matches
        }
    }
}
