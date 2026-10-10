let submit = () => {};

globalThis.tkmmkarenSetSignInSubmit = (fn) => {
    submit = fn;
};

globalThis.tkmmkarenCreateSignInForm = () => {
    const form = document.createElement("form");
    form.className = "tkmmkaren-sign-in";
    form.autocomplete = "on";
    form.innerHTML = `
        <input id="tkmmkaren-username" name="username" type="text" autocomplete="username" placeholder="Username" autocapitalize="none" spellcheck="false">
        <input id="tkmmkaren-password" name="password" type="password" autocomplete="current-password" placeholder="Password">
        <div class="error"></div>
        <button type="submit">Continue</button>`;
    form.addEventListener("submit", (event) => {
        event.preventDefault();
        submit();
    });
    const keepInField = (event) => event.stopPropagation();
    for (const type of ["pointerdown", "pointerup", "mousedown", "mouseup", "click", "keydown", "keyup", "keypress", "beforeinput", "input", "paste"]) {
        form.addEventListener(type, keepInField);
    }
    return form;
};

globalThis.tkmmkarenFieldValue = (form, name) => form.elements.namedItem(name).value;

globalThis.tkmmkarenSetSignInError = (form, message) => {
    form.querySelector(".error").textContent = message ?? "";
};

globalThis.tkmmkarenSetSignInBusy = (form, busy) => {
    form.querySelector("button").disabled = busy;
};
