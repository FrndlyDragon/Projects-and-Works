document.addEventListener("DOMContentLoaded", function (event) {
    var Size = Quill.import('attributors/style/size');
    Size.whitelist = ['10px', '11px', '12px', '14px', '16px', '18px', '20px', '24px']

    const quill = new Quill('#editor', {
        theme: 'snow',
        placeholder: 'Once upon a time...',
        modules: {
            toolbar: '#toolbar',
        }
    });
});

function toggleSidebar() {
    if (document.getElementById("sidebar").style.width == "0%") {
        document.getElementById("sidebar").style.width = "15%";
        let temp = document.getElementById("sidebar-button");
        if (temp) {
            document.getElementById("open-icon").style.transform = "scaleX(-1)"
            temp.style.left = "15%"
        }
    }
    else {
        document.getElementById("sidebar").style.width = "0%";
        let temp = document.getElementById("sidebar-button");
        if (temp) {
            document.getElementById("open-icon").style.transform = "scaleX(1)"
            temp.style.left = "0%"
        }
    }
}
