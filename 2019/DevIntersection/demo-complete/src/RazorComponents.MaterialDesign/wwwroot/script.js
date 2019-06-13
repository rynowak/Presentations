window.BlazorMaterial = {
    button: {
        init: function (elem) {
            mdc.ripple.MDCRipple.attachTo(elem);
        }
    },

    drawer: {
        toggle: function (elem, isOpen) {
            const drawer = mdc.drawer.MDCDrawer.attachTo(elem);
            drawer.open = isOpen;
        }
    },

    topAppBar: {
        init: function (elem, scrollTarget) {
            const topAppBar = mdc.topAppBar.MDCTopAppBar.attachTo(elem);
            if (scrollTarget) {
                topAppBar.setScrollTarget(document.querySelector(scrollTarget));
            }
        }
    },

    dialog: {
        show: function (elem) {
            elem._dialog = elem._dialog || mdc.dialog.MDCDialog.attachTo(elem);
            return new Promise(resolve => {
                const dialog = elem._dialog;
                const callback = event => {
                    dialog.unlisten('MDCDialog:closing', callback);
                    resolve(event.detail.action);
                };
                dialog.listen('MDCDialog:closing', callback);
                dialog.open();
            });
        },
        hide: function (elem, dialogAction) {
            if (elem._dialog) {
                elem._dialog.close(dialogAction || 'dismissed');
            }
        }
    },

    select: {
        init: function (elem) {
            mdc.select.MDCSelect.attachTo(elem);
        }
    },

    textField: {
        init: function (elem) {
            mdc.textField.MDCTextField.attachTo(elem);
        }
    },

    switch: {
        setChecked: function (elem, isChecked) {
            const switchControl = mdc.switchControl.MDCSwitch.attachTo(elem);
            switchControl.checked = isChecked;
        }
    }
};
