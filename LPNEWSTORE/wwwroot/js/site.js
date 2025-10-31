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

// Descarga un archivo a partir de un string de texto
// Uso desde Blazor:
// await JS.InvokeVoidAsync("downloadFileFromText", "productos.csv", csvString, "text/csv;charset=utf-8;");
window.downloadFileFromText = (fileName, content, contentType) => {
    try {
        // Agrega BOM si es CSV o si se indica UTF-8 (mejora compatibilidad con Excel)
        const isCsv = typeof contentType === "string" && /text\/csv/i.test(contentType);
        const isUtf8 = typeof contentType === "string" && /charset\s*=\s*utf-8/i.test(contentType);
        const bom = (isCsv || isUtf8) ? "\uFEFF" : "";

        const blob = new Blob([bom, content ?? ""], {
            type: contentType || "text/plain;charset=utf-8"
        });

        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = fileName || "archivo.txt";
        document.body.appendChild(a);
        a.click();

        // Limpieza
        setTimeout(() => {
            URL.revokeObjectURL(url);
            document.body.removeChild(a);
        }, 0);
    } catch (e) {
        console.error("downloadFileFromText error:", e);
    }
};

window.downloadFromStream = async (fileName, contentStreamReference, contentType) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer], { type: contentType || 'text/plain;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName || "archivo.txt";
    document.body.appendChild(a);
    a.click();
    setTimeout(() => {
        URL.revokeObjectURL(url);
        document.body.removeChild(a);
    }, 0);
};

// wwwroot/js/site.js
window.focusAndHighlightCell = (inputId) => {
    const run = () => {
        const input = document.getElementById(inputId);
        if (!input) return;

        // limpiar previos
        document.querySelectorAll('.kbd-active').forEach(el => el.classList.remove('kbd-active'));
        document.querySelectorAll('.kbd-row-active').forEach(el => el.classList.remove('kbd-row-active'));

        const td = input.closest('td, .mud-table-cell');
        const tr = input.closest('tr, .mud-table-row');

        if (td) td.classList.add('kbd-active');
        if (tr) tr.classList.add('kbd-row-active');

        input.focus();
        if (typeof input.select === 'function') input.select();
        (td ?? input).scrollIntoView({ block: 'nearest', inline: 'nearest' });
    };

    // Esperá al/los próximos frames para ganar la carrera al re-render
    requestAnimationFrame(() => requestAnimationFrame(run));
};




window.atTextEdge = (id) => {
    const el = document.getElementById(id);
    if (!el) return { atStart: false, atEnd: false };
    const start = el.selectionStart ?? 0;
    const end = el.selectionEnd ?? 0;
    const len = el.value?.length ?? 0;
    return { atStart: start === 0 && end === 0, atEnd: start === len && end === len };
};

