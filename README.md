# Important: Make 2 folders called 'Files' and 'Cache' in the root path first
## Configuration in appsettings.json
- `App:BasePath` => The base path of the app, should be matter when deploying to IIS (Example "" or "/cleverconversion")
- `OTCS:Username` => OT username (Example "admin")
- `OTCS:Secret` => OT password (Example "P@ssw0rd")
- `OTCS:Url` => The OT API url (Example "http://192.168.1.225/otcs/cs.exe/api")

## CSUI
```
// CleverConversion Integration - START
var that = this;
const ccConfig = {
    viewerUrl(id, title) {
        return `/cleverconversion/view?nodeid=${id}&filename=${title}`;
    }
};

if (!that.model.attributes.container && isOfficeFile(that.model.attributes.name)) {
    const li = document.createElement("li");
    li.setAttribute("data-csui-command", "none");
    li.setAttribute("role", "none");

    const a = document.createElement("a");

    a.setAttribute("title", "Open in Viewer");
    a.setAttribute("role", "menuitem");
    a.setAttribute("class", "csui-toolitem-icononly");
    a.setAttribute("data-cstabindex", "-1");
    a.setAttribute("aria-label", "Open in Viewer");
    a.setAttribute("id", "infohub-workflow");
    a.onclick = () => openViewer();

    const icon = document.createElement("span");

    icon.classList.add("icon", "icon-toolbar-infohub");

    a.appendChild(icon);

    li.appendChild(a);

    that.$el.find(">ul").prepend(li);
}

function isOfficeFile(fileName) {
    const getFileExt = /(?:\.([^.]+))?$/.exec(fileName)[1];
    if (getFileExt != undefined && ["doc", "docm", "docx", "docxf", "dot", "dotm", "dotx", "epub", "fb2", "fodt", "htm", "html", "mht", "mhtml", "odt", "oform", "ott", "rtf", "stw", "sxw", "txt", "wps", "wpt", "xml", "csv", "et", "ett", "fods", "ods", "ots", "sxc", "xls", "xlsb", "xlsm", "xlsx", "xlt", "xltm", "xltx", "dps", "dpt", "fodp", "odp", "otp", "pot", "potm", "potx", "pps", "ppsm", "ppsx", "ppt", "pptm", "pptx", "sxi", "djvu", "oxps", "pdf", "xps"].includes(getFileExt)) {
        return true;
    }
    return false;
}

function openViewer() {
    const el = document.querySelector(".binf-widgets");
    el.innerHTML += `
        <div id="cc-sidepanel" tabindex="-1" class="csui-sidepanel viewx-sidepanel-doc-preview csui-sidepanel--from-right csui-sidepanel-with-no-mask csui-sidepanel-with-resize csui-sidepanel-custom csui-sidepanel-visible"><div class="csui-sidepanel-container csui-panel-resizable" aria-modal="true" tabindex="0" role="dialog" aria-label="" aria-describedby="sidepanel_description">
            <span id="sidepanel_description" class="binf-sr-only"></span>
            <div class="csui-side-panel-resizer csui-resize-cursor-icon" aria-label="resizer" role="seperator" aria-valuemin="432px" aria-valuemax="100%" tabindex="0" style="touch-action: none;"></div>
            <div class="csui-side-panel-main">
                <div class="ot-iv-Toolbar" role="toolbar" style="height: 3em; z-index: 1;background-color:white">
                    <div class="ot-iv-Toolbar-left" style="position: relative;">
                        <button id="cc-close-btn" style="padding: 8px 16px;">
                            Close
                        </button>
                    </div>
                </div>
                <iframe src="${ccConfig.viewerUrl(that.model.attributes.id, that.model.attributes.name)}" width="100%" height="500px"></iframe>
            </div>
        </div>
    `;
    document.querySelector("#cc-close-btn").addEventListener("click", () => {
        document.querySelector("#cc-sidepanel").remove();
    });
}
// CleverConversion Integration - END
```
