# Important: Make 2 folders called 'Files' and 'Cache' in the root path first (skip if exist)
## Configuration in appsettings.json
- `App:BasePath` => The base path of the app, should be matter when deploying to IIS (Example "" or "/cleverconversion")
- `OTCS:Username` => OT username (Example "admin")
- `OTCS:Secret` => OT password (Example "P@ssw0rd")
- `OTCS:Url` => The OT API url (Example "http://192.168.1.225/otcs/cs.exe/api")

## CSUI (Edit variable `ccConfig`)
```
// CleverConversion Integration - START
var that = this;
const ccConfig = {
    basePath: "/cleverconversion",
    viewerUrl(id, title) {
        return `${this.basePath}/view?nodeid=${id}&filename=${title}`;
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
    if(document.querySelector("#cc-sidepanel") != null) {
        document.querySelector("#cc-sidepanel").remove();
    }

    const el = document.querySelector(".binf-widgets .binf-widgets");
    el.innerHTML += `
        <div id="cc-sidepanel" tabindex="-1" class="csui-sidepanel viewx-sidepanel-doc-preview csui-sidepanel--from-right csui-sidepanel-with-no-mask csui-sidepanel-with-resize csui-sidepanel-custom csui-sidepanel-visible" style="background-color:white">
            <div id="cc-sidepanel-container" class="csui-sidepanel-container csui-panel-resizable" aria-modal="true" tabindex="0" role="dialog" aria-label="" aria-describedby="sidepanel_description" style="visibility:hidden;width: 100vw;background-color: white">
                <span id="sidepanel_description" class="binf-sr-only"></span>
                <div class="csui-side-panel-resizer csui-resize-cursor-icon" aria-label="resizer" role="seperator" aria-valuemin="432px" aria-valuemax="100%" tabindex="0" style="touch-action: none;"></div>
                <div class="csui-side-panel-main">
                    <iframe id="cc-iframe" src="${ccConfig.viewerUrl(that.model.attributes.id, that.model.attributes.name)}" width="100%" height="100%"></iframe>
                </div>
            </div>
        </div>
    `;

    const iframe = document.querySelector("#cc-iframe");
    iframe.addEventListener("load", () => {
        // console.log(document.querySelector('#cc-sidepanel'))
        // document.querySelector("#cc-sidepanel-container").style.width = '50vw';
        // console.log(document.querySelector('#cc-sidepanel'))
        // const interval = setInterval(() => {
        //     const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
        //     const logoText = iframeDoc.querySelector('.gd-header-logo-text');
        //     const logoBrand = iframeDoc.querySelector('.gd-header-logo-brand');
        //     const toolbar = iframeDoc.querySelector("div.gd-header");
        //     const closeBtn = iframeDoc.querySelector("#cc-close-btn");

        //     if (logoText && logoBrand) {
        //         clearInterval(interval);
        //         logoText.src = `${ccConfig.basePath}/img/opentext-logo.png`;
        //         logoBrand.src = `${ccConfig.basePath}/img/6463527.png`;
        //         console.log("Logos updated inside iframe!");
        //     }

        //     if(toolbar && !closeBtn) {
            //         toolbar.innerHTML = `<div _ngcontent-ng-c3276646777="" class="gd-header-col-start" style="margin-right:20px">
        //             <button title="Close" id="cc-close-btn" _ngcontent-ng-c1756903690="" gdbutton="" tooltipposition="bottom" _nghost-ng-c3062361797="" style="background:none" pc10=""><span _ngcontent-ng-c1756903690="" class="material-symbols-outlined ng-star-inserted" style="font-size:18px">close</span></button>    
        //         </div>`;
        //         toolbar.style.height = "32px"
        //         iframeDoc.querySelector("#cc-close-btn").addEventListener("click", () => {
            //             document.querySelector("#cc-sidepanel").remove();
        //         });
        //         iframeDoc.querySelector(".wrapper.ng-star-inserted .ng-star-inserted").style.top = "30px";
        //         iframeDoc.querySelector(".wrapper.ng-star-inserted").style.marginTop = "90px";
        //     }
        // }, 50);
    })
}

window.addEventListener("message", function (event) {
    if (event.data === "closeIframe") {
        console.log("Got message from iframe: closeIframe");
        document.querySelector("#cc-sidepanel").remove();
    }
    // if (event.data === "afterDocsRender") {
    //     console.log("Got message from iframe: afterDocsRender");
    //     document.querySelector("#cc-sidepanel-container").style.width = '50vw';
    // }
    if (event.data === "prepareDocsRender") {
        console.log("Got message from iframe: prepareDocsRender");
        document.querySelector("#cc-sidepanel-container").style.visibility = 'visible';
        document.querySelector("#cc-sidepanel-container").style.width = '50vw';
    }
    // if (event.data === "beforeDocsRender") {
    //     console.log("Got message from iframe: beforeDocsRender");
    //     document.querySelector("#cc-sidepanel-container").style.width = '50vw';
    // }
});
// CleverConversion Integration - END
```
