#include <errno.h>
#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[]) {
    int err;
    if (argc == 1) {
        printf("usage: main <filename>\n");
        return 1;
    }

    FILE *fp;
    char buf[80];

    errno = 0;
    fp = fopen(argv[1], "r");
    if (errno != 0) 
    {
        perror("Error occurred while opening file.\n");
        return 1;
    }

    err = fclose(fp);
    if (err == EINTR)
    {
        printf("Oh dear, something went wrong with fclose()! %s\n", strerror(err));
        return 1;
    }

    return 0;
}