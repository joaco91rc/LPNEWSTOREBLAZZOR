console.log("site.js cargado correctamente");

window.cambiarEstiloSucursal = (archivoCss) => {
    const id = "css-tema-dinamico";
    let existingLink = document.getElementById(id);

    if (existingLink) {
        existingLink.href = archivoCss;
    } else {
        const link = document.createElement("link");
        link.rel = "stylesheet";
        link.href = archivoCss;
        link.id = id;
        document.head.appendChild(link);
    }
}

window.getCookie = function (name) {
    const v = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
    return v ? v.pop() : '';
};

window.setCookie = (name, value, days) => {
    let expires = "";
    if (days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
};

window.tema = {
    guardarModoOscuro: function (valor) {
        localStorage.setItem("darkMode", valor);
    },
    obtenerModoOscuro: function () {
        return localStorage.getItem("darkMode");
    }
};

window.triggerFileInput = (element) => {
    element.click();
};

window.downloadFileFromBytes = (fileName, contentType, base64Data) => {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = `data:${contentType};base64,${base64Data}`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
