var DuringOfRingingTask = new Array();
var TaskInterval = new Array();
var AlarmInterval = new Array();
var currentURL = window.location.href;
var HighInterval;
var ThereIsAnyHigh = false;
var intt = 0;
var intt1 = 0;
var loadtimeout;
var firstTime = sessionStorage.getItem("test_first_time");
var currentDateOfficial = new Date();
var CurrentOffSet = ((currentDateOfficial.getTimezoneOffset() / 60) + 1) * -1;
var CurrentOffSetMinute = 0;
if (currentDateOfficial.getTimezoneOffset() % 60 != 0) {
    var HoursTemp = parseInt((currentDateOfficial.getTimezoneOffset() / 60)) * -1;
    if (((currentDateOfficial.getTimezoneOffset() * -1) - (HoursTemp * 60)) > 0) {
        CurrentOffSetMinute = (currentDateOfficial.getTimezoneOffset() * -1) - (HoursTemp * 60);
        HoursTemp += -1;
        CurrentOffSet = HoursTemp;
    }
}
function UpdateTime() {
    document.cookie = 'TimeZone=; Max-Age=0;';
    var expireTimeZ = new Date();
    expireTimeZ.setFullYear(3000);
    document.cookie = 'TimeZone=' + CreateTimeZoneString() + ';expires=' + expireTimeZ.toUTCString() + ';path=/';
}
setInterval(UpdateTime, 1000);
function MakeKey(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    return result;
}
function GetCookie(Namee) {
    const name = Namee + "=";
    const cDecoded = decodeURIComponent(document.cookie);
    const cArr = cDecoded.split('; ');
    let res;
    cArr.forEach(val => {
        if (val.indexOf(name) === 0) res = val.substring(name.length);
    })

    return res;
}
function CreateTimeZoneString() {
    var CurrentTimeZone = new Date();

    var Day = CurrentTimeZone.getDate();
    var Month = CurrentTimeZone.getMonth()+1;
    var Year = CurrentTimeZone.getFullYear();
    var Hours = CurrentTimeZone.getHours();
    var Minute = CurrentTimeZone.getMinutes();
    var FullString = "";

    if (Month < 10) {
        FullString += "0" + Month.toString() + "/";
    }
    else {
        FullString += Month.toString() + "/";
    }

    if (Day < 10) {
        FullString += "0" + Day.toString()+"/";
    }
    else {
        FullString += Day.toString() + "/";
    }

    FullString += Year.toString() + "/";

    if (Hours < 10) {
        FullString += "0" + Hours.toString() + "/";
    }
    else {
        FullString += Hours.toString() + "/";
    }

    if (Minute < 10) {
        FullString += "0" + Minute.toString();
    }
    else {
        FullString += Minute.toString();
    }

    return FullString;
}
$(document).ready(function () {
    if (!firstTime) {
        sessionStorage.setItem("test_first_time", "1");
        var target1 = null;
        target1 = document.getElementById("loadtest");
        var cook = false;
        if (typeof GetCookie("TimeZone") !== "undefined") {
            cook = true;
        }
        document.cookie = 'TimeZone=; Max-Age=0;';
        var expireTimeZ = new Date();
        expireTimeZ.setFullYear(3000);
        document.cookie = 'TimeZone=' + CreateTimeZoneString() + ';expires=' + expireTimeZ.toUTCString() + ';path=/';

        if (typeof GetCookie("RealKey") === "undefined") {
            var Key = MakeKey(248);

            var expireTime = new Date();
            var expireTime1 = new Date();
            expireTime.setFullYear(3000);
            expireTime1.setDate(expireTime1.getDate() + 2);

            document.cookie = 'RealKey=' + Key + ';expires=' + expireTime.toUTCString() + ';path=/';
            document.cookie = 'CopyRealKey=' + Key + ';expires=' + expireTime1.toUTCString() + ';path=/';
        }

        if (typeof GetCookie("RealKey") !== "undefined" && typeof GetCookie("CopyRealKey") === "undefined") {
            var Key = GetCookie("RealKey");

            var expireTime1 = new Date();
            expireTime1.setDate(expireTime1.getDate() + 2);

            document.cookie = 'CopyRealKey=' + Key + ';expires=' + expireTime1.toUTCString() + ';path=/';
        }


        if (FindString("en-US", currentURL) == true) {
            buttonformdiv.action = "en-US/Home/Index";
        }
        if (FindString("ja-JP", currentURL) == true) {
            buttonformdiv.action = "ja-JP/Home/Index";
        }

        if (target1 != null && typeof target1 !== "undefined") {
            InitialStorage();
        }

        var numberoftasksandalarms = 0;
        var TaskMainList = JSON.parse(sessionStorage.getItem("TaskList"));
        var AlarmMainList = JSON.parse(sessionStorage.getItem("AlarmList"));
        numberoftasksandalarms = TaskMainList.length + AlarmMainList.length;


        var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
        var isFirefox = typeof InstallTrigger !== 'undefined';
        var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && window['safari'].pushNotification));
        var isIE = /*@cc_on!@*/false || !!document.documentMode;
        var isEdge = !isIE && !!window.StyleMedia;
        var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
        var isEdgeChromium = isChrome && (navigator.userAgent.indexOf("Edg") != -1);
        var isBlink = (isChrome || isOpera) && !!window.CSS;
        var output = 'Detecting browsers by ducktyping:<hr>';
        output += 'isFirefox: ' + isFirefox + '<br>';
        output += 'isChrome: ' + isChrome + '<br>';
        output += 'isSafari: ' + isSafari + '<br>';
        output += 'isOpera: ' + isOpera + '<br>';
        output += 'isIE: ' + isIE + '<br>';
        output += 'isEdge: ' + isEdge + '<br>';
        output += 'isEdgeChromium: ' + isEdgeChromium + '<br>';
        output += 'isBlink: ' + isBlink + '<br>';
        var TestAudio = new Audio("../../Sounds/TaskSound1.mp3");
        TestAudio.volume = 0;
        var TestAudioFirefox = new Audio("../../Sounds/AlarmSound5.wav");
        if (isFirefox == true) {
            TestAudioFirefox.play();
            TestAudioFirefox.volume = 0;
        }
        if (isFirefox == true && !TestAudioFirefox.paused == false) {
            if (target1 != null && typeof target1 !== "undefined" && numberoftasksandalarms > 0) {
                CreateAudioExceptionMessage("Firefox");
            }
        }
        else {
            if (isIE == false) {
                var playPromise = TestAudio.play().catch(function (params) {
                    if (isChrome == true && numberoftasksandalarms > 0) {
                        CreateAudioExceptionMessage("Chrome");
                    }
                });
            }
        }
    }
    else {
        if (typeof GetCookie("RealKey") === "undefined") {
            var Key = MakeKey(248);

            var expireTime = new Date();
            var expireTime1 = new Date();
            expireTime.setFullYear(3000);
            expireTime1.setDate(expireTime1.getDate() + 2);

            document.cookie = 'RealKey=' + Key + ';expires=' + expireTime.toUTCString() + ';path=/';
            document.cookie = 'CopyRealKey=' + Key + ';expires=' + expireTime1.toUTCString() + ';path=/';
        }

        if (typeof GetCookie("RealKey") !== "undefined" && typeof GetCookie("CopyRealKey") === "undefined") {
            var Key = GetCookie("RealKey");

            var expireTime1 = new Date();
            expireTime1.setDate(expireTime1.getDate() + 2);

            document.cookie = 'CopyRealKey=' + Key + ';expires=' + expireTime1.toUTCString() + ';path=/';
        }

        document.cookie = 'TimeZone=; Max-Age=0;';
        var expireTimeZ = new Date();
        expireTimeZ.setFullYear(3000);
        document.cookie = 'TimeZone=' + CreateTimeZoneString() + ';expires=' + expireTimeZ.toUTCString() + ';path=/';

        var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
        var isFirefox = typeof InstallTrigger !== 'undefined';
        var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && window['safari'].pushNotification));
        var isIE = /*@cc_on!@*/false || !!document.documentMode;
        var isEdge = !isIE && !!window.StyleMedia;
        var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
        var isEdgeChromium = isChrome && (navigator.userAgent.indexOf("Edg") != -1);
        var isBlink = (isChrome || isOpera) && !!window.CSS;

        var target1 = null;
        target1 = document.getElementById("loadtest");


        if (target1 != null && typeof target1 !== "undefined") {
            InitialStorage();
        }

        var numberoftasksandalarms = 0;
        var TaskMainList = JSON.parse(sessionStorage.getItem("TaskList"));
        var AlarmMainList = JSON.parse(sessionStorage.getItem("AlarmList"));
        numberoftasksandalarms = TaskMainList.length + AlarmMainList.length;

        var TestAudio = new Audio("../../Sounds/TaskSound1.mp3");
        TestAudio.volume = 0;
        var TestAudioFirefox = new Audio("../../Sounds/AlarmSound5.wav");
        if (isFirefox == true) {
            TestAudioFirefox.play();
            TestAudioFirefox.volume = 0;
        }
        if (isFirefox == true && !TestAudioFirefox.paused == false) {
            if (target1 != null && typeof target1 !== "undefined" && numberoftasksandalarms > 0) {
                CreateAudioExceptionMessage("Firefox");
            }
        }
        else {
            if (isIE == false) {
                var playPromise = TestAudio.play().catch(function (params) {
                    if (isChrome == true && numberoftasksandalarms > 0) {
                        CreateAudioExceptionMessage("Chrome");
                    }
                });
            }
        }
    }
    var element = null;
    element = document.getElementById("smallsearchdivright");
    var element1 = null;
    element1 = document.getElementById("smallsearchdiv");
    if (element != null && typeof element !== "undefined") {
        var widt = $(window).width();
        if (widt > 1199) {
            element.style.display = "none";
        }
    }
    if (element1 != null && typeof element1 !== "undefined") {
        var widt = $(window).width();
        if (widt > 1199) {
            element1.style.display = "none";
        }
    }
    var elemenet = $(".mainmobilediv")[0];

    if (typeof elemenet !== "undefined" && elemenet != null) {
        var maindivheight = elemenet.offsetHeight * 0.02;

        var newtaskalarmparent = $(".addnewtaskalarmparent")[0];
        if (typeof newtaskalarmparent !== "undefined" && newtaskalarmparent != null) {
            var newtaskheight = newtaskalarmparent.offsetHeight;

            var realelement = document.getElementById("smallsearchdiv");

            if (typeof realelement !== "undefined" && realelement != null) {
                var bot = maindivheight + newtaskheight + 40;
                realelement.style.top = bot + 'px';
            }
        }
    }
    if (typeof elemenet !== "undefined" && elemenet != null) {
        var maindivheight = elemenet.offsetHeight * 0.02;

        var newtaskalarmparent = $(".addnewtaskalarmparent")[0];
        if (typeof newtaskalarmparent !== "undefined" && newtaskalarmparent != null) {
            var newtaskheight = newtaskalarmparent.offsetHeight;

            var realelement = document.getElementById("smallsearchdivright");

            if (typeof realelement !== "undefined" && realelement != null) {
                var bot = maindivheight + newtaskheight + 40;
                realelement.style.top = bot + 'px';
            }
        }
    }

    var mainelement = document.getElementById("maincontainer");

    if (mainelement != null && typeof mainelement !== "undefined") {
        mainelement.style.backgroundImage = "url(/Images/MainImage15.jpg)";
    }

    var chilnewtaskdiv = $("#chilnewtaskdiv");
    var jschilnewtaskdiv = document.getElementById("chilnewtaskdiv");
    if (typeof chilnewtaskdiv !== "undefined" && chilnewtaskdiv != null && typeof jschilnewtaskdiv !== "undefined" && jschilnewtaskdiv != null) {
        var screenwidth = $(window).width();
        var screenheight = $(window).height();
        if (screenwidth > 992) {
            if (screenheight - 300 < 870) {
                jschilnewtaskdiv.style.height = screenheight - 300 + 'px';
            }
            else {
                jschilnewtaskdiv.style.height = 870 + 'px';
            }
        }
        else {
            if (screenheight - 360 < 870) {
                jschilnewtaskdiv.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdiv.style.height = 870 + 'px';
            }
        }
    }

    var chilnewtaskdiv1 = $("#chilnewtaskdiv");
    var jschilnewtaskdivalarm = document.getElementsByClassName("alarmnewdiv")[0];
    if (typeof chilnewtaskdiv1 !== "undefined" && chilnewtaskdiv1 != null && typeof jschilnewtaskdivalarm !== "undefined" && jschilnewtaskdivalarm != null) {
        var screenwidth = $(window).width();
        var screenheight = $(window).height();
        if (screenwidth > 992) {
            if (screenheight - 300 < 610) {
                jschilnewtaskdivalarm.style.height = screenheight - 300 + 'px';
            }
            else {
                jschilnewtaskdivalarm.style.height = 610 + 'px';
            }
        }
        else {
            if (screenheight - 360 < 610) {
                jschilnewtaskdivalarm.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdivalarm.style.height = 610 + 'px';
            }
        }
    }

    var maincont = $("#maincontainer");
    if (typeof maincont !== "undefined" && maincont != null) {
        var widthmain = $("#maincontainer").width();
        var heightmain = $("#maincontainer").height();
        var addiv1 = document.getElementById("addiv1");
        var addiv2 = document.getElementById("addiv2");
        if (typeof addiv1 !== "undefined" && addiv1 != null && typeof addiv2 !== "undefined" && addiv2 != null) {
            var halfwidthmain = (widthmain / 4) - 225;
            var halfwidthmain2 = (widthmain / 4) - 225;
            var implementheighttop = (heightmain / 2) - 50;
            addiv1.style.left = halfwidthmain + 'px';
            addiv1.style.top = implementheighttop + 'px';
            addiv2.style.right = halfwidthmain2 + 'px';
            addiv2.style.top = implementheighttop + 'px';

        }
    }

    var loaddivs = document.getElementsByClassName("loaddiv");
    if (loaddivs.length == 0) {
        var loaddiv = document.createElement("div");
        loaddiv.classList.add("loaddiv");
        document.body.prepend(loaddiv);
    }

    var target1 = null;
    target1 = document.getElementById("loadtest");

    if (target1 == null || typeof target1 === "undefined") {
        loadtimeout = setTimeout(EnableLoad, 700);
    }
    else {
        loadtimeout = setTimeout(EnableLoad,1000);
    }


    audio = new Audio("../../Sounds/TaskSound1.mp3");
    audioalarm = new Audio("../../Sounds/TaskSound1.mp3");

    var bigselect = null;
    if (document.getElementById("select-profession") != null) {
        bigselect = document.getElementById("select-profession").value;
    }
    var bigselectright = null;
    if (document.getElementsByClassName("rightselectdesktop")[0] != null) {
        bigselectright = document.getElementsByClassName("rightselectdesktop")[0].value;
    }
    var smallselect = null;
    if (document.getElementsByClassName("smallprofessionselect")[0] != null) {
        smallselect = document.getElementsByClassName("smallprofessionselect")[0].value;
    }
    var smallselectright = null;
    if (document.getElementsByClassName("smallprofessionselectalarm")[0] != null) {
        smallselectright = document.getElementsByClassName("smallprofessionselectalarm")[0].value;
    }

    var allelements = document.getElementsByClassName("sel__placeholder");
    var elementletfdesktop = null;
    var elementletfmobile = null;
    var elementrightmobile = null;
    var elementrightdesktop = null;

    if (allelements.length > 0) {
        elementletfdesktop = allelements[0];
        elementletfmobile = allelements[1];
        elementrightmobile = allelements[2];
        elementrightdesktop = allelements[3];
    }

    if (bigselect == 1 || bigselect == 0) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "All";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "All";
        }
    }
    if (bigselect == 2) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "In processing";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "In processing";
        }
    }
    if (bigselect == 3) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Upcoming";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "Upcoming";
        }
    }
    if (bigselect == 4) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Finished";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "Finished";
        }
    }
    if (bigselectright == 0 || bigselectright == 1) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "All";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "All";
        }
    }
    if (bigselectright == 2) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "Active";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "Active";
        }
    }
    if (bigselectright == 3) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "Inactive";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "Inactive";
        }
    }
    if (smallselectright == 0 || smallselectright == 1) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "All";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "All";
        }
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "All";
        }
    }
    if (smallselectright == 2) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "Active";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "Active";
        }
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Active";
        }
    }
    if (smallselectright == 3) {
        if (typeof elementrightmobile !== "undefined" && elementrightmobile != null) {
            elementrightmobile.innerHTML = "Inactive";
        }
        if (typeof elementrightdesktop !== "undefined" && elementrightdesktop != null) {
            elementrightdesktop.innerHTML = "Inactive";
        }
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Inactive";
        }
    }
    if (smallselect == 1 || smallselect == 0) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "All";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "All";
        }
    }
    if (smallselect == 2) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "In processing";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "In processing";
        }
    }
    if (smallselect == 3) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Upcoming";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "Upcoming";
        }
    }
    if (smallselect == 4) {
        if (typeof elementletfdesktop !== "undefined" && elementletfdesktop != null) {
            elementletfdesktop.innerHTML = "Finished";
        }
        if (typeof elementletfmobile !== "undefined" && elementletfmobile != null) {
            elementletfmobile.innerHTML = "Finished";
        }
    }

    var elemenet = $(".mainmobilediv")[0];
    if (typeof elemenet !== "undefined" && elemenet != null) {
        var maindivheight = elemenet.offsetHeight * 0.02;

        var newtaskalarmparent = $(".addnewtaskalarmparent")[0];
        if (typeof newtaskalarmparent !== "undefined" && newtaskalarmparent != null) {
            var newtaskheight = newtaskalarmparent.offsetHeight;

            var realelement = document.getElementById("smallsearchdiv");

            if (typeof realelement !== "undefined" && realelement != null) {
                var bot = maindivheight + newtaskheight + 40;
                realelement.style.top = bot + 'px';
            }
        }
    }

    var desktophomemain = $("#desktophome");
    if (typeof desktophomemain !== "undefined" && desktophomemain != null) {
        var widthmain = $("#desktophome").width();
        var heightmain = $("#desktophome").height();
        var desktophomead = document.getElementById("desktophomead");
        if (typeof desktophomead !== "undefined" && desktophomead != null) {
            var halfwidthmain = (widthmain / 2) - 135;
            var implementheighttop = heightmain - 280;
            desktophomead.style.left = halfwidthmain + 'px';
            desktophomead.style.top = implementheighttop + 'px';
        }
    }


    var obj = null;
    obj = document.getElementById("customcreatefor");
    var obj1 = null;
    obj1 = document.getElementById("oncecreated");

    if (obj != null && obj1 != null && typeof obj !== "undefined" && typeof obj1 !== "undefined") {
        if ($("#createforr").val() == 2) {
            obj.style.display = "block";
            obj1.style.display = "none";
        }
        else {
            obj.style.display = "none";
            obj1.style.display = "block";
        }
    }

    var obj = null;
    obj = document.getElementById("customcreateforalarm");

    if (obj != null && typeof obj !== "undefined") {
        if ($("#createforralarm").val() == 2) {
            obj.style.display = "block";
        }
        else {
            obj.style.display = "none";
        }
    }

    var footerheight = $("footer").height();

    var maincookieelement = document.getElementById("maincookie");
    var buttoncookie = document.getElementById("buttoncookie");
    var windowwidth = $(window).width();
    var maincookieelementheight = document.getElementById("maincookie").offsetHeight;

    if (maincookieelement != null && typeof maincookieelement !== "undefined" && buttoncookie != null && typeof buttoncookie !== "undefined") {
        var prop = footerheight + 10;
        maincookieelement.style.bottom = prop + 'px';

        var prop1 = 0;
        
        if (windowwidth < 921) {
            prop1 = (maincookieelementheight / 1.4) - buttoncookie.offsetHeight;
        }
        if (windowwidth > 920) {
            prop1 = (maincookieelementheight / 1.3) - buttoncookie.offsetHeight;
        }

        if (windowwidth < 730) {
            buttoncookie.style.right = 10 + 'px';
        }
        var btncook = document.getElementById("buttoncookie");
        var cookcons = document.getElementById("cookieConsent");
        if (windowwidth < 658) {
            if (cookcons != null && typeof cookcons !== "undefined") {
                if (cookcons.children[0] != null && typeof cookcons.children[0] !== "undefined") {
                    cookcons.children[0].style.display = "block";
                    cookcons.children[0].style.width = 100 + '%';
                    cookcons.style.textAlign = "center";
                }
            }
            var btncook = document.getElementById("buttoncookie");
            if (btncook != null && typeof btncook !== "undefined") {
                btncook.style.position = "relative";
                btncook.style.top = 10 + 'px';
                btncook.style.right = 0 + 'px';
                btncook.style.width = 60 + 'px';
            }
        }
        if (windowwidth >= 658) {
            if (btncook != null && typeof btncook !== "undefined") {
                btncook.style.position = "absolute";
                btncook.style.right = 10 + '%';
                btncook.style.width = 10 + 'px';
            }
            if (cookcons != null && typeof cookcons !== "undefined") {
                if (cookcons.children[0] != null && typeof cookcons.children[0] !== "undefined") {
                    cookcons.children[0].style.display = "inline-block";
                    cookcons.children[0].style.width = 85 + '%';
                    cookcons.style.textAlign = "left";
                }
            }
            
            buttoncookie.style.top = prop1+ 'px';
        }
    }

    $(".closetaskdiv").click(function (params) {
        var closedivelement = this;
        var Key = null;
        Key = closedivelement.children[1].value;
        var TaskName = null;
        TaskName = closedivelement.children[2].value;
        if (Key != null && TaskName != null) {
            DeleteTask(Key, TaskName, "task");
        }
    });
    $(".closealarmdiv").click(function (params) {
        var closedivelement = this;
        var Key = null;
        Key = closedivelement.children[1].value;
        var TaskName = null;
        TaskName = closedivelement.children[2].value;
        if (Key != null && TaskName != null) {
            DeleteTask(Key, TaskName, "alarm");
        }
    });
});
$(document).click(function (params) {
    var listofall = document.getElementsByClassName("notificationdiv");
    var parentfordelete = document.getElementsByClassName("parentnotificationdiv")[0];
    for (var i = 0; i < listofall.length; i++) {
        if (listofall[i].children[0].children[0].innerText == "Allow sounds") {
            if (parentfordelete != null && typeof parentfordelete !== "undefined") {
                parentfordelete.removeChild(listofall[i]);

                var list = document.getElementsByClassName("notificationdiv");
                if (list.length == 0) {
                    var mainunderbody = document.getElementById("mainunderbody");
                    mainunderbody.removeChild(parentfordelete);
                }
            }
        }
    }
});
function InitialStorage() {
    var ListOfTasksHTML = document.getElementsByClassName("tasknotification");

    var ListOfTaskNotification = new Array();
    for (var i = 0; i < ListOfTasksHTML.length; i++) {

        const Task = new Object();
        Task.Key = ListOfTasksHTML[i].children[0].value;
        Task.TaskName = ListOfTasksHTML[i].children[1].value;

        var StartDate = ListOfTasksHTML[i].children[2].value;
        var StartTime = ListOfTasksHTML[i].children[3].value;
        
        
        var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
        var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1));
        var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
        var StartHour = parseInt(StartTime[0].toString(1) + StartTime[1].toString(1)) + CurrentOffSet;
        var StartMinute = parseInt(StartTime[3].toString(1) + StartTime[4].toString(1)) + CurrentOffSetMinute;

        var StartDateInput = new Date(StartYear, StartMonth-1, StartDay, StartHour, StartMinute);
        Task.StartTimeDate = StartDateInput;

        var EndDate = ListOfTasksHTML[i].children[4].value;
        var EndTime = ListOfTasksHTML[i].children[5].value;

        var EndYear = parseInt(EndDate[0].toString(1) + EndDate[1].toString(1) + EndDate[2].toString(1) + EndDate[3].toString(1));
        var EndMonth = parseInt(EndDate[5].toString(1) + EndDate[6].toString(1));
        var EndDay = parseInt(EndDate[8].toString(1) + EndDate[9].toString(1));
        var EndHour = parseInt(EndTime[0].toString(1) + EndTime[1].toString(1)) + CurrentOffSet;
        var EndMinute = parseInt(EndTime[3].toString(1) + EndTime[4].toString(1)) + CurrentOffSetMinute;

        var EndDateInput = new Date(EndYear, EndMonth-1, EndDay, EndHour, EndMinute);
        Task.EndTimeDate = EndDateInput;

        var LastNotDate = ListOfTasksHTML[i].children[6].value;
        var LastNotTime = ListOfTasksHTML[i].children[7].value;

        var LastNotYear = parseInt(LastNotDate[0].toString(1) + LastNotDate[1].toString(1) + LastNotDate[2].toString(1) + LastNotDate[3].toString(1));
        var LastNotMonth = parseInt(LastNotDate[5].toString(1) + LastNotDate[6].toString(1));
        var LastNotDay = parseInt(LastNotDate[8].toString(1) + LastNotDate[9].toString(1));
        var LastNotHour = parseInt(LastNotTime[0].toString(1) + LastNotTime[1].toString(1));
        var LastNotMinute = parseInt(LastNotTime[3].toString(1) + LastNotTime[4].toString(1));

        var LastNotDateInput = new Date(LastNotYear, LastNotMonth-1, LastNotDay, LastNotHour, LastNotMinute);
        Task.LastNotTimeDate = LastNotDateInput;

        Task.NotificationEvery = ListOfTasksHTML[i].children[8].value;
        Task.Priority = ListOfTasksHTML[i].children[9].value;

        if (Task.Priority == "High") {
            DuringOfRingingTask.push(0);
        }
        if (Task.Priority == "Normal") {
            DuringOfRingingTask.push(15);
        }
        if (Task.Priority == "Low") {
            DuringOfRingingTask.push(5);
        }
        
        TaskInterval.push(0);

        Task.SoundName = ListOfTasksHTML[i].children[10].value;
        Task.Days = ListOfTasksHTML[i].children[11].value;
        Task.CreatedFor = ListOfTasksHTML[i].children[12].value;
        Task.Stats = ListOfTasksHTML[i].children[13].value;
        Task.AcceptedNotification = ListOfTasksHTML[i].children[14].value;
        Task.Rated = ListOfTasksHTML[i].children[15].value;
        
        
        ListOfTaskNotification.push(Task);
    }

    sessionStorage.setItem('TaskList', JSON.stringify(ListOfTaskNotification));
    
    var ListOfAlarmHTML = document.getElementsByClassName("alarmnotification");
    var ListOfAlarmNotification = new Array();

    for (var i = 0; i < ListOfAlarmHTML.length; i++) {

        const Alarm = new Object();

        Alarm.Key = ListOfAlarmHTML[i].children[0].value;
        Alarm.AlarmName = ListOfAlarmHTML[i].children[1].value;

        var StartDate = ListOfAlarmHTML[i].children[2].value;
        var StartTime = ListOfAlarmHTML[i].children[3].value;

        var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
        var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1));
        var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
        var StartHour = parseInt(StartTime[0].toString(1) + StartTime[1].toString(1)) + CurrentOffSet;
        var StartMinute = parseInt(StartTime[3].toString(1) + StartTime[4].toString(1)) + CurrentOffSetMinute;

        var StartDateInput = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);
        Alarm.RingingTime = StartDateInput;

        var LastSnoozeDate = ListOfAlarmHTML[i].children[4].value;
        var LastSnoozeTime = ListOfAlarmHTML[i].children[5].value;

        var LastSnoozeStartYear = parseInt(LastSnoozeDate[0].toString(1) + LastSnoozeDate[1].toString(1) + LastSnoozeDate[2].toString(1) + LastSnoozeDate[3].toString(1));
        var LastSnoozeStartMonth = parseInt(LastSnoozeDate[5].toString(1) + LastSnoozeDate[6].toString(1));
        var LastSnoozeStartDay = parseInt(LastSnoozeDate[8].toString(1) + LastSnoozeDate[9].toString(1));
        var LastSnoozeStartHour = parseInt(LastSnoozeTime[0].toString(1) + LastSnoozeTime[1].toString(1)) + CurrentOffSet;
        var LastSnoozeStartMinute = parseInt(LastSnoozeTime[3].toString(1) + LastSnoozeTime[4].toString(1)) + CurrentOffSetMinute;

        var LastSnoozeDateInput = new Date(LastSnoozeStartYear, LastSnoozeStartMonth, LastSnoozeStartDay, LastSnoozeStartHour, LastSnoozeStartMinute);
        Alarm.LastSnoozeDateTime = LastSnoozeDateInput;

        Alarm.RingDuration = ListOfAlarmHTML[i].children[6].value;
        Alarm.SnoozeDuration = ListOfAlarmHTML[i].children[7].value;
        Alarm.SoundName = ListOfAlarmHTML[i].children[8].value;
        Alarm.Days = ListOfAlarmHTML[i].children[9].value;
        Alarm.CreatedFor = ListOfAlarmHTML[i].children[10].value;

        Alarm.LastRinging = ListOfAlarmHTML[i].children[11].value;
        Alarm.Ringing = false;
        AlarmInterval.push(0);

        ListOfAlarmNotification.push(Alarm);
    }

    sessionStorage.setItem('AlarmList', JSON.stringify(ListOfAlarmNotification));
}
function CreateAudioExceptionMessage(Browser) {
    var parentnotdiv = null;
    var notdiv = null;
    parentnotdiv = document.getElementsByClassName("parentnotificationdiv");

    if (parentnotdiv.length == 0) {
        parentnotdiv = document.createElement("div");
        parentnotdiv.classList.add("parentnotificationdiv");
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.prepend(parentnotdiv);
    }
    else {
        parentnotdiv = parentnotdiv[0];
    }
    notdiv = document.createElement("div");
    notdiv.classList.add("notificationdiv");
    parentnotdiv.appendChild(notdiv);
    notdiv.style.zIndex = 50000;
    var titlenotdiv = document.createElement("div");
    titlenotdiv.classList.add("titlenotdiv");
    notdiv.appendChild(titlenotdiv);

    if (Browser == "Firefox") {
        notdiv.style.height = 390 + 'px';
        titlenotdiv.style.paddingTop = 21 + 'px';
    }
    else {
        notdiv.style.height = 275 + 'px';
        titlenotdiv.style.paddingTop = 12 + 'px';
    }

    var titlespan = document.createElement("span");
    titlenotdiv.appendChild(titlespan);
    var titlespaname = "Allow sounds";
    
    titlespan.innerHTML = titlespaname;
    titlespan.style.fontWeight = "bold";

    var questionparentdiv = document.createElement("div");
    questionparentdiv.classList.add("questionparentdiv");
    notdiv.appendChild(questionparentdiv);
    questionparentdiv.style.paddingTop = 10 + 'px';

    var questiondiv = document.createElement("div");
    questiondiv.classList.add("questiondiv");
    questionparentdiv.appendChild(questiondiv);

    var standardquestion = document.createElement("span");
    standardquestion.innerHTML = "Allow notification sounds!"+'<br>'+'<br>';
    standardquestion.style.fontWeight = "bold";
    standardquestion.classList.add("questiontext");
    questiondiv.appendChild(standardquestion);

    var tasknamespan = document.createElement("span");
    tasknamespan.style.fontWeight = "bold";

    tasknamespan.classList.add("questiontext");
    questiondiv.appendChild(tasknamespan);
    var onlytasktext = document.createElement("span");
    onlytasktext.style.fontWeight = "bold";
    
    var onlytasktextname = "Our website uses sound signals during notification and in accordance with the privacy policy we need to get permission through your interaction on the website!" + '<br>';
    onlytasktext.classList.add("questiontext");
    onlytasktext.style.fontSize = 12 + 'px';
    onlytasktext.innerHTML = onlytasktextname;
    questiondiv.appendChild(onlytasktext);

    if (Browser != "Firefox") {
        var br = document.createElement("br");
        questiondiv.appendChild(br);
    }

    if (Browser == "Firefox") {
        var br = document.createElement("br");
        questiondiv.appendChild(br);
        var Permissionspan = document.createElement("span");
        Permissionspan.style.fontWeight = "bold";
        Permissionspan.classList.add("questiontext");
        var Permisiontext = "Allow sounds permanently:" + '<br>';
        Permissionspan.style.fontSize = 14 + 'px';
        Permissionspan.innerHTML = Permisiontext;
        questiondiv.appendChild(Permissionspan);
    }

    if (Browser == "Firefox") {
        var Routespan = document.createElement("span");
        Routespan.style.fontWeight = "bold";
        Routespan.classList.add("questiontext");
        var Routetext = "Menu -> Settings -> Privacy & Security -> Permissions -> Autoplay -> Allow Audio and Video" + '<br>'+'<br>';
        Routespan.style.fontSize = 11 + 'px';
        Routespan.innerHTML = Routetext;
        questiondiv.appendChild(Routespan);

    }
    var Routespan1 = document.createElement("span");
    Routespan1.style.fontWeight = "bold";
    Routespan1.classList.add("questiontext");
    var Routetext1 = ""; 
    if (Browser == "Firefox") {
        Routetext1 = "or allow sounds temporarily by clicking the 'Allow sounds!'.";
    }
    else {
        Routetext1 = "Allow sounds by clicking the 'Allow sounds!'.";
    }
    Routespan1.style.fontSize = 13 + 'px';
    Routespan1.innerHTML = Routetext1;
    questiondiv.appendChild(Routespan1);

    var parentnotdiv = document.createElement("div");
    parentnotdiv.classList.add("parentnotbuttondiv");
    notdiv.appendChild(parentnotdiv);
    if (Browser == "Firefox") {
        parentnotdiv.style.top = 87 + '%';
    }
    else {
        parentnotdiv.style.top = 82 + '%';
    }

    var buttondiv = document.createElement("div");
    buttondiv.classList.add("buttondiv");
    buttondiv.style.width = 100 + '%';
    parentnotdiv.appendChild(buttondiv);

    var buttonformdiv = document.createElement("form");
    buttonformdiv.classList.add("buttondivform");
    buttondiv.appendChild(buttonformdiv);
    var buttonyes = document.createElement("button");
    buttonyes.type = "button";
    buttonyes.classList.add("button");
    buttonyes.classList.add("btn-primary");
    buttonyes.classList.add("buttonyes");
    buttonyes.textContent = "Allow sounds!";
    buttonformdiv.appendChild(buttonyes);
    var inputkey = document.createElement("input");
    inputkey.type = "hidden";
    inputkey.value = Browser;
    buttonformdiv.appendChild(inputkey);
    buttonyes.addEventListener("click", CloseAllowSoundDiv);
}
function CloseAllowSoundDiv() {
    var currentelement = this;
    var fordelete = currentelement.parentElement.parentElement.parentElement.parentElement;
    var parentfordelete = document.getElementsByClassName("parentnotificationdiv")[0];

    if (parentfordelete != null && typeof parentfordelete !== "undefined") {
        parentfordelete.removeChild(fordelete);

        var list = document.getElementsByClassName("notificationdiv");
        if (list.length == 0) {
            var mainunderbody = document.getElementById("mainunderbody");
            mainunderbody.removeChild(parentfordelete);
        }
    }
    var TestAudioFirefox = new Audio("../../Sounds/AlarmSound5.wav");

    TestAudioFirefox.play();
    TestAudioFirefox.volume = 0;

    if (!TestAudioFirefox.paused == true) {
        MainDisplayNotification();
    }
}
$("#task").click(function (params) {
    location.href = "/Task/Index";
});
$("#alarm").click(function (params) {
    location.href = "/Alarm/Index";
});
$(".smallsearchleftdiv").click(function (params) {
    var elem = document.getElementById("smallsearchdiv");
    if (intt == 0) {
        if (typeof elem !== "undefined" && elem != null) {
            elem.style.display = "inline-block";
            intt++;
        }
    }
    else {
        if (typeof elem !== "undefined" && elem != null) {
            elem.style.display = "none";
            intt--;
        }
    }
});
$(".smallsearchrightdiv").click(function (params) {
    var elem = document.getElementById("smallsearchdivright");
    if (intt1 == 0) {
        if (typeof elem !== "undefined" && elem != null) {
            elem.style.display = "inline-block";
            intt1++;
        }
    }
    else {
        if (typeof elem !== "undefined" && elem != null) {
            elem.style.display = "none";
            intt1--;
        }
    }
});
function Close() {
    var elem = document.getElementById("smallsearchdiv");
    if (typeof elem !== "undefined" && elem != null) {
        if (elem.style.display == "inline-block") {
            elem.style.display = "none";
        }
        else {
            elem.style.display = "inline-block";
        }
        if (elem.style.display == "none") {
            elem.style.display = "inline-block";
        }
        else {
            elem.style.display = "none";
        }
    }
}
function Close1() {
    var elem = document.getElementById("smallsearchdivright");
    if (typeof elem !== "undefined" && elem != null) {
        if (elem.style.display == "inline-block") {
            elem.style.display = "none";
        }
        else {
            elem.style.display = "inline-block";
        }
        if (elem.style.display == "none") {
            elem.style.display = "inline-block";
        }
        else {
            elem.style.display = "none";
        }
    }
}
$('.sel').each(function () {
    $(this).children('select').css('display', 'none');

    var $current = $(this);

    var currentelement = this;

    $(this).find('option').each(function (i) {
        if (i == 0) {

            
            var firstdiv = document.createElement("div");
            firstdiv.classList.add("sel__box");
            currentelement.insertBefore(firstdiv, currentelement.children[0]);

            //$current.prepend($('<div>', {
            //    class: $current.attr('class').replace(/sel/g, 'sel__box')
            //}));

            var placeholder = $(this).text();
           
            var firstspan = document.createElement("span");
            firstspan.classList.add("sel__placeholder");
            firstspan.innerHTML = placeholder;
            firstspan.placeholder = placeholder;
            currentelement.insertBefore(firstspan, firstdiv);

            //$current.prepend($('<span>', {
            //    class: $current.attr('class').replace(/sel/g, 'sel__placeholder'),
            //    text: placeholder,
            //    'data-placeholder': placeholder
            //}));

            return;
        }
        $current.children('div').append($('<span>', {
            class: $current.attr('class').replace(/sel/g, 'sel__box__options'),
            text: $(this).text()
        }));
    });
});
$('.sel').click(function () {
    $(this).toggleClass('active');
});
$('.sel__box__options').click(function () {
    var parentelement = this.parentElement;
    
    $(this).siblings('.sel__box__options').removeClass('selected');
    $(this).addClass('selected');

    var element = parentelement.parentElement.children[0];

    var rightselectdesktop = document.getElementsByClassName("rightselectdesktop")[0];
    var smallprofesionalselect = document.getElementsByClassName("smallprofessionselectalarm")[0];
    var selectprofesion = document.getElementById("select-profession");
    var smallprofesionselect = document.getElementsByClassName("smallprofessionselect")[0];

    if (parentelement.parentElement.classList.length == 4) {
        if (this.innerText == "All") {
            if (typeof rightselectdesktop !== "undefined" && rightselectdesktop!=null) {
                rightselectdesktop.selectedIndex = "1";
            }
            if (typeof smallprofesionalselect !== "undefined" && smallprofesionalselect != null) {
                smallprofesionalselect.selectedIndex = "1";
            }
        }
        if (this.innerText == "Active") {
            if (typeof rightselectdesktop !== "undefined" && rightselectdesktop != null) {
                rightselectdesktop.selectedIndex = "2";
            }
            if (typeof smallprofesionalselect !== "undefined" && smallprofesionalselect != null) {
                smallprofesionalselect.selectedIndex = "2";
            }
        }

        if (this.innerText == "Inactive") {
            if (typeof rightselectdesktop !== "undefined" && rightselectdesktop != null) {
                rightselectdesktop.selectedIndex = "3";
            }
            if (typeof smallprofesionalselect !== "undefined" && smallprofesionalselect != null) {
                smallprofesionalselect.selectedIndex = "3";
            }
        }
    }
    else {
        if (this.innerText == "All") {
            if (typeof selectprofesion !== "undefined" && selectprofesion != null) {
                selectprofesion.selectedIndex = "1";
            }
            if (typeof smallprofesionselect !== "undefined" && smallprofesionselect != null) {
                smallprofesionselect.selectedIndex = "1";
            }
        }
        if (this.innerText == "In processing") {
            if (typeof selectprofesion !== "undefined" && selectprofesion != null) {
                selectprofesion.selectedIndex = "2";
            }
            if (typeof smallprofesionselect !== "undefined" && smallprofesionselect != null) {
                smallprofesionselect.selectedIndex = "2";
            }
        }
        if (this.innerText == "Upcoming") {
            if (typeof selectprofesion !== "undefined" && selectprofesion != null) {
                selectprofesion.selectedIndex = "3";
            }
            if (typeof smallprofesionselect !== "undefined" && smallprofesionselect != null) {
                smallprofesionselect.selectedIndex = "3";
            }
        }
        if (this.innerText == "Finished") {
            if (typeof selectprofesion !== "undefined" && selectprofesion != null) {
                selectprofesion.selectedIndex = "4";
            }
            if (typeof smallprofesionselect !== "undefined" && smallprofesionselect != null) {
                smallprofesionselect.selectedIndex = "4";
            }
        }
    }
    element.innerHTML = this.innerText;
});
$("#createforr").change(function (parms) {
    var obj = document.getElementById("customcreatefor");
    var obj1 = document.getElementById("oncecreated");
    if (obj != null && obj1 != null && typeof obj !== "undefined" && typeof obj1 !== "undefined") {
        if ($("#createforr").val() == 2) {
            obj.style.display = "block";
            obj1.style.display = "none";
        }
        else {
            obj.style.display = "none";
            obj1.style.display = "block";
        }
    }


    if ($("#createforr").val() == 1) {
        if (IsAllFalse() == false) {
            if (typeof document.getElementById("weekday-mon") !== "undefined" && document.getElementById("weekday-mon") != null) {
                document.getElementById("weekday-mon").checked = true;
            }
        }
    }
});
var audio = new Audio("../../Sounds/TaskSound1.mp3");
$("#tasksound").click(function (parms) {
    if ($("#tasksound").val() == 1) {
        PlaySound("TaskSound1");
    }
    if ($("#tasksound").val() == 2) {
        PlaySound("TaskSound2");
    }
    if ($("#tasksound").val() == 3) {
        PlaySound("TaskSound3");
    }
    if ($("#tasksound").val() == 4) {
        PlaySound("TaskSound4");
    }
    if ($("#tasksound").val() == 5) {
        PlaySound("TaskSound5");
    }
    if ($("#tasksound").val() == 6) {
        PlaySound("TaskSound6");
    }
    if ($("#tasksound").val() == 7) {
        PlaySound("TaskSound7");
    }
});
function PlaySound(SoundName) {
    var root = "../../Sounds/" + SoundName + ".mp3";
    audio.pause();
    audio.src = root;
    audio.play();
}
$("#endtimecustomvalue").change(function (parms) {
    var starttimecousteeval = null;
    starttimecousteeval = document.getElementById("starttimecustomvalidation").firstChild;

    var starttimevalue = document.getElementById("startimecustomvalue").value;

    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];

    var starthoursnumber = parseInt(starthours);
    var startminutesnumber = parseInt(startminutes);

    var endtimevalue = document.getElementById("endtimecustomvalue").value;

    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];

    var endhoursnumber = parseInt(endhours);
    var endminutesnumber = parseInt(endminutes);

    if (starttimecousteeval != null && typeof starttimecousteeval !== "undefined") {
        if (starthoursnumber < endhoursnumber && starttimecousteeval.innerHTML == "Start time can't be same or higher than end time!") {
            starttimecousteeval.innerHTML = "";
        }
        else {
            if (starthoursnumber == endhoursnumber && starttimecousteeval.innerHTML == "Start time can't be same or higher than end time!") {
                if (startminutesnumber < endminutesnumber) {
                    starttimecousteeval.innerHTML = "";
                }
            }
        }
    }

});
$("#startimecustomvalue").change(function (parms) {
    var starttimecousteeval = null;
    starttimecousteeval = document.getElementById("endtimecustomvalidation").firstChild;

    var starttimevalue = document.getElementById("startimecustomvalue").value;

    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];

    var starthoursnumber = parseInt(starthours);
    var startminutesnumber = parseInt(startminutes);

    var endtimevalue = document.getElementById("endtimecustomvalue").value;

    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];

    var endhoursnumber = parseInt(endhours);
    var endminutesnumber = parseInt(endminutes);

    if (starttimecousteeval != null && typeof starttimecousteeval !== "undefined") {
        if (starthoursnumber < endhoursnumber && starttimecousteeval.innerHTML == "End time can't be same or lower than start time!") {
            starttimecousteeval.innerHTML = "";
        }
        else {
            if (starthoursnumber == endhoursnumber && starttimecousteeval.innerHTML == "End time can't be same or lower than start time!") {
                if (startminutesnumber < endminutesnumber) {
                    starttimecousteeval.innerHTML = "";
                }
            }
        }
    }

});
$("#endtime").change(function (parms) {
    var starttimeonceval = null;
    starttimeonceval = document.getElementById("starttimeonceval").firstChild;

    var enddateval = null;
    enddateval = document.getElementById("enddateval").firstChild;

    var startdatevall = null;
    startdatevall = document.getElementById("startdateval").firstChild;

    var startdatevalue = new Date(document.getElementById("startdatevalue").value);

    var starttimevalue = document.getElementById("starttimevalue").value;

    var startyear = startdatevalue.getFullYear();
    var startmonth = startdatevalue.getMonth();
    var startday = startdatevalue.getDay();
    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];
    var startfulldate = new Date(startyear, startmonth, startday, starthours, startminutes);

    var enddatevalue = new Date(document.getElementById("enddate").value);

    var endtimevalue = document.getElementById("endtime").value;

    var endyear = enddatevalue.getFullYear();
    var endmonth = enddatevalue.getMonth();
    var endday = enddatevalue.getDay();
    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];
    var endfulldate = new Date(endyear, endmonth, endday, endhours, endminutes);

    if (enddateval != null && IsValid(startfulldate, endfulldate) && typeof enddateval !== "undefined") {
        if (enddateval.innerHTML == "End time can't be same or lower than start time!")
            enddateval.innerHTML = "";
    }

    if (starttimeonceval != null && IsValid(startfulldate, endfulldate) && starttimeonceval.innerHTML == "Start time can't be same or higher than end time!") {
        starttimeonceval.innerHTML = "";
    }

    if (startdatevall != null && IsValid(startfulldate, endfulldate) && startdatevall.innerHTML == "Start time can't be same or higher than end time!") {
        startdatevall.innerHTML = "";
    }
});
$("#startdatevalue").change(function (parms) {
    var starttimeonceval = null;
    starttimeonceval = document.getElementById("endtimeoneval").firstChild;

    var startdatevall = null;
    startdatevall = document.getElementById("enddateval").firstChild;

    var enddateval = null;
    enddateval = document.getElementById("starttimeonceval").firstChild;

    var startdatevalue = new Date(document.getElementById("startdatevalue").value);

    var starttimevalue = document.getElementById("starttimevalue").value;

    var startyear = startdatevalue.getFullYear();
    var startmonth = startdatevalue.getMonth();
    var startday = startdatevalue.getDay();
    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];
    var startfulldate = new Date(startyear, startmonth, startday, starthours, startminutes);

    var enddatevalue = new Date(document.getElementById("enddate").value);

    var endtimevalue = document.getElementById("endtime").value;

    var endyear = enddatevalue.getFullYear();
    var endmonth = enddatevalue.getMonth();
    var endday = enddatevalue.getDay();
    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];
    var endfulldate = new Date(endyear, endmonth, endday, endhours, endminutes);

    if (enddateval != null && IsValid(startfulldate, endfulldate)) {
        if (enddateval.innerHTML == "Start time can't be same or higher than end time!")
            enddateval.innerHTML = "";
    }

    if (starttimeonceval != null && IsValid(startfulldate, endfulldate) && starttimeonceval.innerHTML == "End time can't be same or lower than start time!") {
        starttimeonceval.innerHTML = "";
    }

    if (startdatevall != null && IsValid(startfulldate, endfulldate) && startdatevall.innerHTML == "End time can't be same or lower than start time!") {
        startdatevall.innerHTML = "";
    }
});
$("#starttimevalue").change(function (parms) {
    var starttimeonceval = null;
    starttimeonceval = document.getElementById("endtimeoneval").firstChild;

    var startdatevall = null;
    startdatevall = document.getElementById("enddateval").firstChild;

    var enddateval = null;
    enddateval = document.getElementById("startdateval").firstChild;

    var startdatevalue = new Date(document.getElementById("startdatevalue").value);

    var starttimevalue = document.getElementById("starttimevalue").value;

    var startyear = startdatevalue.getFullYear();
    var startmonth = startdatevalue.getMonth();
    var startday = startdatevalue.getDay();
    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];
    var startfulldate = new Date(startyear, startmonth, startday, starthours, startminutes);

    var enddatevalue = new Date(document.getElementById("enddate").value);

    var endtimevalue = document.getElementById("endtime").value;

    var endyear = enddatevalue.getFullYear();
    var endmonth = enddatevalue.getMonth();
    var endday = enddatevalue.getDay();
    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];
    var endfulldate = new Date(endyear, endmonth, endday, endhours, endminutes);

    if (enddateval != null && IsValid(startfulldate, endfulldate)) {
        if (enddateval.innerHTML == "Start time can't be same or higher than end time!")
            enddateval.innerHTML = "";
    }

    if (starttimeonceval != null && IsValid(startfulldate, endfulldate) && starttimeonceval.innerHTML == "End time can't be same or lower than start time!") {
        starttimeonceval.innerHTML = "";
    }

    if (startdatevall != null && IsValid(startfulldate, endfulldate) && startdatevall.innerHTML == "End time can't be same or lower than start time!") {
        startdatevall.innerHTML = "";
    }
});
$("#enddate").change(function (parms) {
    var starttimeonceval = null;
    starttimeonceval = document.getElementById("starttimeonceval").firstChild;

    var startdatevall = null;
    startdatevall = document.getElementById("startdateval").firstChild;

    var enddateval = null;
    enddateval = document.getElementById("endtimeoneval").firstChild;

    var startdatevalue = new Date(document.getElementById("startdatevalue").value);

    var starttimevalue = document.getElementById("starttimevalue").value;

    var startyear = startdatevalue.getFullYear();
    var startmonth = startdatevalue.getMonth();
    var startday = startdatevalue.getDay();
    var starthours = starttimevalue[0] + starttimevalue[1];
    var startminutes = starttimevalue[3] + starttimevalue[4];
    var startfulldate = new Date(startyear, startmonth, startday, starthours, startminutes);

    var enddatevalue = new Date(document.getElementById("enddate").value);

    var endtimevalue = document.getElementById("endtime").value;

    var endyear = enddatevalue.getFullYear();
    var endmonth = enddatevalue.getMonth();
    var endday = enddatevalue.getDay();
    var endhours = endtimevalue[0] + endtimevalue[1];
    var endminutes = endtimevalue[3] + endtimevalue[4];
    var endfulldate = new Date(endyear, endmonth, endday, endhours, endminutes);

    if (enddateval != null && IsValid(startfulldate, endfulldate)) {
        if (enddateval.innerHTML == "End time can't be same or lower than start time!")
            enddateval.innerHTML = "";
    }

    if (starttimeonceval != null && IsValid(startfulldate, endfulldate) && starttimeonceval.innerHTML == "Start time can't be same or higher than end time!") {
        starttimeonceval.innerHTML = "";
    }

    if (startdatevall != null && IsValid(startfulldate, endfulldate) && startdatevall.innerHTML == "Start time can't be same or higher than end time!") {
        startdatevall.innerHTML = "";
    }
});
function IsValid(StartDate, EndDate) {
    if (EndDate > StartDate) {
        return true;
    }
    return false;
}
$("#weekday-mon").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-tue").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-wed").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-thu").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-fri").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-sat").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
$("#weekday-sun").change(function (parms) {
    if (IsAllFalse() == false) {
        document.getElementById("daysval").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysval").innerHTML = "";
    }
});
function IsAllFalse() {
    var m = document.getElementById("weekday-mon").checked;
    var t = document.getElementById("weekday-tue").checked;
    var w = document.getElementById("weekday-wed").checked;
    var th = document.getElementById("weekday-thu").checked;
    var f = document.getElementById("weekday-fri").checked;
    var sa = document.getElementById("weekday-sat").checked;
    var s = document.getElementById("weekday-sun").checked;

    if (m == false && t == false && w == false && th == false && f == false && sa == false && s == false) {
        return false;
    }
    else {
        return true;
    }
}
function EnableLoad() {
    var list = document.getElementsByClassName("loaddiv");
    if (list.length > 0) {
        var elementforremove = list[0];
        document.body.removeChild(elementforremove);
    }
    clearTimeout(loadtimeout);
}
function OnClickDate (event) {
    var element = event.target;
    element.style.borderColor = "orange";
    element.style.borderRadius = "0px";
}
function toggleForm() {
    document.getElementById("register").classList.toggle("active");
}
function DeleteDivUnAuth(Key,ValueName) {
    if (ValueName == "alarm") {
        var AlarmListStored = JSON.parse(sessionStorage.getItem("AddedAlarmList"));
        var AlarmListStoredTemp = new Array();
        
        for (var i = 0; i < AlarmListStored.length; i++) {
            if (AlarmListStored[i].Key != Key) {
                AlarmListStoredTemp.push(AlarmListStored[i]);
            }
        }
        sessionStorage.setItem('AddedAlarmList', JSON.stringify(AlarmListStoredTemp));
    }
}
function DeleteTask(Key, TaskName,ValueName) {
    var parentnotdiv = null;
    var notdiv = null;
    parentnotdiv = document.getElementsByClassName("parentnotificationdiv");
   
    if (parentnotdiv.length == 0) {
        parentnotdiv = document.createElement("div");
        parentnotdiv.classList.add("parentnotificationdiv");
        var firstelement = document.getElementById("topbar");
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.insertBefore(parentnotdiv, firstelement);
    }
    notdiv = document.createElement("div");
    notdiv.classList.add("notificationdiv");
    parentnotdiv.appendChild(notdiv);
    var titlenotdiv = document.createElement("div");
    titlenotdiv.classList.add("titlenotdiv");
    notdiv.appendChild(titlenotdiv);

    var titlespan = document.createElement("span");
    titlenotdiv.appendChild(titlespan);
    var titlespaname = "Delete ";
    for (var i = 0; i < ValueName.length; i++) {
        titlespaname += ValueName[i];
    }
    titlespan.innerHTML = titlespaname;
    titlespan.style.fontWeight = "bold";

    var questionparentdiv = document.createElement("div");
    questionparentdiv.classList.add("questionparentdiv");
    notdiv.appendChild(questionparentdiv);

    var questiondiv = document.createElement("div");
    questiondiv.classList.add("questiondiv");
    questionparentdiv.appendChild(questiondiv);

    var standardquestion = document.createElement("span");
    standardquestion.innerHTML = "Are you sure you want to delete ";
    standardquestion.style.fontWeight = "bold";
    standardquestion.classList.add("questiontext");
    questiondiv.appendChild(standardquestion);

    var tasknamespan = document.createElement("span");
    tasknamespan.style.fontWeight = "bold";
    
    tasknamespan.classList.add("questiontext");
    questiondiv.appendChild(tasknamespan);

    var temp;
    if (TaskName.length > 35) {
        temp = "";
        for (var i = 0; i < 36; i++) {
            temp += TaskName[i];
        }
        temp += "...";
        tasknamespan.innerHTML = temp;
    }
    else {
        tasknamespan.innerHTML = TaskName;
    }
    


    var onlytasktext = document.createElement("span");
    onlytasktext.style.fontWeight = "bold";
    var onlytasktextname = " ";
    for (var i = 0; i < ValueName.length; i++) {
        onlytasktextname += ValueName[i];
    }
    onlytasktextname += "?";
    onlytasktext.classList.add("questiontext");
    onlytasktext.innerHTML = onlytasktextname;
    questiondiv.appendChild(onlytasktext);

    var windowwidth = $(window).width();
    var padd = 0;
    var heightofparentdiv = questionparentdiv.offsetHeight;
    var heightofquestiondiv = questiondiv.offsetHeight;
    if (windowwidth < 501) {
        if (heightofquestiondiv > 50) {
            padd = heightofparentdiv - heightofquestiondiv - 45;
        }
        else {
            if (heightofquestiondiv > 25) {
                padd = heightofparentdiv - (heightofquestiondiv * 2) - 16;
            }
            else {
                padd = heightofparentdiv - (heightofquestiondiv * 2) - 42;
            }
        }
    }
    else {
        if (heightofquestiondiv > 50) {
            padd = heightofparentdiv - (heightofquestiondiv * 2) - 35;
        }
        else {
            if (heightofquestiondiv > 25) {
                padd = heightofparentdiv - (heightofquestiondiv * 2) - 15;
            }
            else {
                padd = heightofparentdiv - (heightofquestiondiv * 2) - 42;
            }
        }
    }
    questionparentdiv.style.paddingTop = padd + 'px';

    var parentnotdiv = document.createElement("div");
    parentnotdiv.classList.add("parentnotbuttondiv");
    notdiv.appendChild(parentnotdiv);

    var buttondiv = document.createElement("div");
    buttondiv.classList.add("buttondiv");
    parentnotdiv.appendChild(buttondiv);

    var buttonformdiv = document.createElement("form");
    buttonformdiv.classList.add("buttondivform");
    if (ValueName != null) {
        var widt=$(window).width();
        if (ValueName == "task") {
            if (widt < 993) {
                buttonformdiv.action = "/Task/DeleteTaskMobile";
            }
            else {
                buttonformdiv.action = "/Task/DeleteTask";
            }
        }
        else {
            if (widt < 993) {
                buttonformdiv.action = "/Alarm/DeleteAlarmMobile";
            }
            else {
                if (FindString("en-US", currentURL) == true) {
                    buttonformdiv.action = "en-US/Alarm/DeleteAlarm";
                }
                if (FindString("ja-JP", currentURL) == true) {
                    buttonformdiv.action = "ja-JP/Alarm/DeleteAlarm";
                }
            }
        }
    }
    buttonformdiv.method = "post";
    buttondiv.appendChild(buttonformdiv);
    var inputkeyelement = document.createElement("input");
    inputkeyelement.type = "hidden";
    inputkeyelement.name = "Key";
    inputkeyelement.value = Key;
    buttonformdiv.appendChild(inputkeyelement);
    var buttonyes = document.createElement("button");
    buttonyes.type = "submit";
    buttonyes.classList.add("button");
    buttonyes.classList.add("btn-primary");
    buttonyes.classList.add("buttonyes");
    buttonyes.textContent = "Yes!";
    buttonformdiv.appendChild(buttonyes);
    var buttonformright = document.createElement("form");
    buttonformright.classList.add("buttondivformright");
    buttondiv.appendChild(buttonformright);
    var buttonno = document.createElement("a");
    buttonno.classList.add("buttonno");
    buttonno.classList.add("button");
    buttonno.classList.add("btn-danger");
    buttonno.textContent = "No!";
    buttonno.style.paddingLeft = "10px";
    buttonno.style.paddingRight = "10px";
    buttonno.style.paddingTop = "4px";
    buttonno.style.paddingBottom = "3px";
    buttonno.addEventListener("click", CloseDeleteDiv);
    buttonformright.appendChild(buttonno);
}
function CloseDeleteDiv() {
    var element = this;
    var buttonformrightdiv = element.parentElement;
    buttonformrightdiv.removeChild(element);
    var buttondiv = buttonformrightdiv.parentElement;
    var buttondivform = buttondiv.children[0];
    var firstbuttondivform = buttondivform.children[0];
    buttondivform.removeChild(firstbuttondivform);
    buttondiv.removeChild(buttonformrightdiv);
    buttondiv.removeChild(buttondivform);
    var parentbuttondiv = buttondiv.parentElement;
    parentbuttondiv.removeChild(buttondiv);
    var notificationdivmain = parentbuttondiv.parentElement;
    notificationdivmain.removeChild(parentbuttondiv);
    var titlenotdiv = notificationdivmain.children[0];
    var titlespan = titlenotdiv.children[0];
    titlenotdiv.removeChild(titlespan);
    var questionparentdiv = notificationdivmain.children[1];
    var questiondiv = questionparentdiv.children[0];
    var firstspan = questiondiv.children[0];
    var secondspan = questiondiv.children[1];
    var thirdspan = questiondiv.children[2];
    questiondiv.removeChild(firstspan);
    questiondiv.removeChild(secondspan);
    questiondiv.removeChild(thirdspan);
    questionparentdiv.removeChild(questiondiv);
    notificationdivmain.removeChild(questionparentdiv);
    notificationdivmain.removeChild(titlenotdiv);
    var parentofparent = notificationdivmain.parentElement;
    parentofparent.removeChild(notificationdivmain);
    var list = document.getElementsByClassName("notificationdiv");
    if (list.length == 0) {
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.removeChild(parentofparent);
    }
}
$("#createforralarm").change(function (parms) {
    var obj = document.getElementById("customcreateforalarm");

    if (typeof obj !== "undefined" && obj != null) {
        if ($("#createforralarm").val() == 2) {
            obj.style.display = "block";
        }
        else {
            obj.style.display = "none";
        }

        if ($("#createforralarm").val() == 1) {
            if (IsAllFalseAlarm() == false) {
                document.getElementById("weekday-mona").checked = true;
            }
        }
    }
});
$("#weekday-mona").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-tuea").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-weda").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-thua").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-fria").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-sata").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
$("#weekday-suna").change(function (parms) {
    if (IsAllFalseAlarm() == false) {
        document.getElementById("daysvalalarm").innerHTML = "At least one day must be marked!";
    }
    else {
        document.getElementById("daysvalalarm").innerHTML = "";
    }
});
var audioalarm;
var alarmsoundclosed = true;
var lastsound = 1;
var cleartm;
$("#AlarmSound").click(function (parms) {
    if (alarmsoundclosed == true || lastsound != $("#AlarmSound").val()) {
        if ($("#AlarmSound").val() == 1) {
            PlaySoundAlarm("AlarmSound1", "mp3");
            lastsound = 1;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 2) {
            PlaySoundAlarm("AlarmSound2", "mp3");
            lastsound = 2;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 3) {
            PlaySoundAlarm("AlarmSound3", "mp3");
            lastsound = 3;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 4) {
            PlaySoundAlarm("AlarmSound4", "mp3");
            lastsound = 4;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 5) {
            PlaySoundAlarm("AlarmSound5", "wav");
            lastsound = 5;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 6) {
            PlaySoundAlarm("AlarmSound6", "wav");
            lastsound = 6;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        if ($("#AlarmSound").val() == 7) {
            PlaySoundAlarm("AlarmSound7", "mp3");
            lastsound = 7;
            clearTimeout(cleartm);
            StopAlarmSound();
        }
        alarmsoundclosed = false;
    }
    else {
        clearTimeout(cleartm);
        alarmsoundclosed = true;
        audioalarm.pause();
    }
});
function PlaySoundAlarm(SoundName, Ext) {
    var root = "../../Sounds/" + SoundName + "." + Ext;
    audioalarm.pause();
    audioalarm.src = root;
    audioalarm.play();
}
function IsAllFalseAlarm() {
    var m = document.getElementById("weekday-mona").checked;
    var t = document.getElementById("weekday-tuea").checked;
    var w = document.getElementById("weekday-weda").checked;
    var th = document.getElementById("weekday-thua").checked;
    var f = document.getElementById("weekday-fria").checked;
    var sa = document.getElementById("weekday-sata").checked;
    var s = document.getElementById("weekday-suna").checked;

    if (m == false && t == false && w == false && th == false && f == false && sa == false && s == false) {
        return false;
    }
    else {
        return true;
    }
}
function StopAlarmSound() {
    cleartm = setTimeout(StopAlarmS, 3000);
}
function StopAlarmS() {
    audioalarm.pause();
}
$(window).resize(function () {
    
    var element = null;
    element = document.getElementById("smallsearchdivright");
    var element1 = null;
    element1 = document.getElementById("smallsearchdiv");
    if (element != null && typeof element !== "undefined") {
        var widt = $(window).width();
        if (widt > 1199) {
            element.style.display = "none";
        }
    }
    if (element1 != null && typeof element1 !== "undefined") {
        var widt = $(window).width();
        if (widt > 1199) {
            element1.style.display = "none";
        }
    }
    var elemenet = $(".mainmobilediv")[0];

    if (typeof elemenet !== "undefined" && elemenet != null) {
        var maindivheight = elemenet.offsetHeight * 0.02;

        var newtaskalarmparent = $(".addnewtaskalarmparent")[0];
        if (typeof newtaskalarmparent !== "undefined" && newtaskalarmparent != null) {
            var newtaskheight = newtaskalarmparent.offsetHeight;

            var realelement = document.getElementById("smallsearchdiv");

            if (typeof realelement !== "undefined" && realelement != null) {
                var bot = maindivheight + newtaskheight + 40;
                realelement.style.top = bot + 'px';
            }
        }
    }
    if (typeof elemenet !== "undefined" && elemenet != null) {
        var maindivheight = elemenet.offsetHeight * 0.02;
        
        var newtaskalarmparent = $(".addnewtaskalarmparent")[0];
        if (typeof newtaskalarmparent !== "undefined" && newtaskalarmparent != null) {
            var newtaskheight = newtaskalarmparent.offsetHeight;

            var realelement = document.getElementById("smallsearchdivright");

            if (typeof realelement !== "undefined" && realelement != null) {
                var bot = maindivheight + newtaskheight + 40;
                realelement.style.top = bot + 'px';
            }
        }
    }

    var desktophomemain = $("#desktophome");
    if (typeof desktophomemain !== "undefined" && desktophomemain != null) {
        var widthmain = $("#desktophome").width();
        var heightmain = $("#desktophome").height();
        var desktophomead = document.getElementById("desktophomead");
        if (typeof desktophomead !== "undefined" && desktophomead != null) {
            var halfwidthmain = (widthmain / 2) - 135;
            var implementheighttop = heightmain - 280;
            desktophomead.style.left = halfwidthmain + 'px';
            desktophomead.style.top = implementheighttop + 'px';
        }
    }

    var maincont = $("#maincontainer");
    if (typeof maincont !== "undefined" && maincont != null) {
        var widthmain = $("#maincontainer").width();
        var heightmain = $("#maincontainer").height();
        var addiv1 = document.getElementById("addiv1");
        var addiv2 = document.getElementById("addiv2");
        if (typeof addiv1 !== "undefined" && addiv1 != null && typeof addiv2 !== "undefined" && addiv2 != null) {
            var halfwidthmain = (widthmain / 4) - 225;
            var halfwidthmain2 = (widthmain / 4) - 225;
            var implementheighttop = (heightmain/2)-50;
            addiv1.style.left = halfwidthmain + 'px';
            addiv1.style.top = implementheighttop + 'px';
            addiv2.style.right = halfwidthmain2 + 'px';
            addiv2.style.top = implementheighttop + 'px';
            
        }
    }

    var chilnewtaskdiv = $("#chilnewtaskdiv");
    var jschilnewtaskdiv = document.getElementById("chilnewtaskdiv");
    if (typeof chilnewtaskdiv !== "undefined" && chilnewtaskdiv != null && typeof jschilnewtaskdiv !== "undefined" && jschilnewtaskdiv != null) {
        var screenwidth = $(window).width();
        var screenheight = $(window).height();
        if (screenwidth > 992) {
            if (screenheight - 300 < 870) {
                jschilnewtaskdiv.style.height = screenheight - 300 + 'px';
            }
            else {
                jschilnewtaskdiv.style.height = 870 + 'px';
            }
        }
        else {
            if (screenheight - 360 < 870) {
                jschilnewtaskdiv.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdiv.style.height = 870 + 'px';
            }
        }
        if (screenwidth == 993) {
            if (screenheight - 360 < 870) {
                jschilnewtaskdiv.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdiv.style.height = 870 + 'px';
            }
        }
    }

    var chilnewtaskdiv1 = $("#chilnewtaskdiv");
    var jschilnewtaskdivalarm = document.getElementsByClassName("alarmnewdiv")[0];
    if (typeof chilnewtaskdiv1 !== "undefined" && chilnewtaskdiv1 != null && typeof jschilnewtaskdivalarm !== "undefined" && jschilnewtaskdivalarm != null) {
        var screenwidth = $(window).width();
        var screenheight = $(window).height();
        if (screenwidth > 992) {
            if (screenheight - 300 < 610) {
                jschilnewtaskdivalarm.style.height = screenheight - 300 + 'px';
            }
            else {
                jschilnewtaskdivalarm.style.height = 610 + 'px';
            }
        }
        else {
            if (screenheight - 360 < 610) {
                jschilnewtaskdivalarm.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdivalarm.style.height = 610 + 'px';
            }
        }
        if (screenwidth == 993) {
            if (screenheight - 360 < 610) {
                jschilnewtaskdivalarm.style.height = screenheight - 360 + 'px';
            }
            else {
                jschilnewtaskdivalarm.style.height = 610 + 'px';
            }
        }
    }

    var windowwidth = $(window).width();

    var questionparentdivlist = document.getElementsByClassName("questionparentdiv");

    if (questionparentdivlist.length > 0) {
        if (windowwidth < 501) {
            for (var i = 0; i < questionparentdivlist.length; i++) {
                for (var j = 0; j < questionparentdivlist[i].children[0].children.length; j++) {
                    if (questionparentdivlist[i].children[0].children[j] != null && typeof questionparentdivlist[i].children[0].children[j] !== "undefined") {
                        if (questionparentdivlist[i].parentElement.children[0].children[0].innerText != "Allow sounds") {
                            questionparentdivlist[i].children[0].children[j].style.fontSize = 13 + 'px';
                            questionparentdivlist[i].children[0].children[j].style.fontWeight = 'bold';
                        }
                        else {
                            if (questionparentdivlist[i].parentElement.children[2].children[0].children[0].children[1].value == "Firefox") {
                                questionparentdivlist[i].parentElement.style.height = 410 + 'px';
                                questionparentdivlist[i].parentElement.children[0].style.paddingTop = 21 + 'px';
                                questionparentdivlist[i].parentElement.children[2].style.top = 90 + '%';
                            }
                            else {
                                questionparentdivlist[i].parentElement.style.height = 270 + 'px';
                                questionparentdivlist[i].parentElement.children[0].style.paddingTop = 12 + 'px';
                                questionparentdivlist[i].parentElement.children[2].style.top = 85 + '%';
                            }
                        }
                    }
                }
                if (questionparentdivlist[i] != null && typeof questionparentdivlist[i] !== "undefined") {
                    if (questionparentdivlist[i].parentElement.children[0].children[0].innerText != "Allow sounds") {
                        var heightofparentdiv = questionparentdivlist[i].offsetHeight;
                        var heightofquestiondiv = questionparentdivlist[i].children[0].offsetHeight;
                        var padd = 0;
                        
                        if (heightofquestiondiv > 50) {
                            
                            padd = heightofparentdiv - heightofquestiondiv - 45;
                        }
                        else {
                            
                            if (heightofquestiondiv > 25) {
                                padd = heightofparentdiv - (heightofquestiondiv * 2) - 15;
                            }
                            else {
                                padd = heightofparentdiv - (heightofquestiondiv * 2) - 42;
                            }
                        }
                        questionparentdivlist[i].style.paddingTop = padd + 'px';
                    }
                }
            }
        }
        else {
            for (var i = 0; i < questionparentdivlist.length; i++) {
                for (var j = 0; j < questionparentdivlist[i].children[0].children.length; j++) {
                    if (questionparentdivlist[i].children[0].children[j] != null && typeof questionparentdivlist[i].children[0].children[j] !== "undefined") {
                        if (questionparentdivlist[i].parentElement.children[0].children[0].innerText != "Allow sounds") {
                            questionparentdivlist[i].children[0].children[j].style.fontSize = 14 + 'px';
                            questionparentdivlist[i].children[0].children[j].style.fontWeight = "bold";
                        }
                        else {
                            if (questionparentdivlist[i].parentElement.children[2].children[0].children[0].children[1].value == "Firefox") {
                                questionparentdivlist[i].parentElement.style.height = 340 + 'px';
                                questionparentdivlist[i].parentElement.children[0].style.paddingTop = 17 + 'px';
                                questionparentdivlist[i].parentElement.children[2].style.top = 87 + '%';
                            }
                            else {
                                questionparentdivlist[i].parentElement.style.height = 250 + 'px';
                                questionparentdivlist[i].parentElement.children[0].style.paddingTop = 10 + 'px';
                                questionparentdivlist[i].parentElement.children[2].style.top = 82 + '%';
                            }
                        }
                    }
                }
                if (questionparentdivlist[i] != null && typeof questionparentdivlist[i] !== "undefined") {
                    if (questionparentdivlist[i].parentElement.children[0].children[0].innerText != "Allow sounds") {
                        var heightofparentdiv = questionparentdivlist[i].offsetHeight;
                        var heightofquestiondiv = questionparentdivlist[i].children[0].offsetHeight;
                        var padd = 0;
                        if (heightofquestiondiv > 50) {
                            padd = heightofparentdiv - (heightofquestiondiv * 2) - 35;
                        }
                        else {
                            if (heightofquestiondiv > 25) {
                                padd = heightofparentdiv - (heightofquestiondiv * 2) - 15;
                            }
                            else {
                                padd = heightofparentdiv - (heightofquestiondiv * 2) - 42;
                            }
                        }
                        questionparentdivlist[i].style.paddingTop = padd + 'px';
                    }
                }
            }
        }
    }

    var footerheight = $("footer").height();
    
    var maincookieelement = document.getElementById("maincookie");
    var buttoncookie = document.getElementById("buttoncookie");

    var maincookieelementheight = document.getElementById("maincookie").offsetHeight;

    if (maincookieelement != null && typeof maincookieelement !== "undefined" && buttoncookie != null && typeof buttoncookie!=="undefined") {
        var prop = footerheight + 10;
        maincookieelement.style.bottom = prop + 'px';

        var prop1 = 0;
        if (windowwidth < 921) {
            prop1=(maincookieelementheight/1.4) - buttoncookie.offsetHeight;
        }
        if (windowwidth > 920) {
            prop1 = (maincookieelementheight / 1.3) - buttoncookie.offsetHeight;
        }

        if (windowwidth < 730) {
            buttoncookie.style.right = 10 + 'px';
        }
        var btncook = document.getElementById("buttoncookie");
        var cookcons = document.getElementById("cookieConsent");
        if (windowwidth < 658) {
            if (cookcons != null && typeof cookcons !== "undefined") {
                if (cookcons.children[0] != null && typeof cookcons.children[0] !== "undefined") {
                    cookcons.children[0].style.display = "block";
                    cookcons.children[0].style.width = 100 + '%';
                    cookcons.style.textAlign = "center";
                }
            }
            var btncook = document.getElementById("buttoncookie");
            if (btncook != null && typeof btncook !== "undefined") {
                btncook.style.position = "relative";
                btncook.style.top = 10+'px';
                btncook.style.right = 0 + 'px';
                btncook.style.width = 60 + 'px';
            }
        }
        if (windowwidth >= 658) {
            if (btncook != null && typeof btncook !== "undefined") {
                btncook.style.position = "absolute";
                btncook.style.right = 10 + '%';
                btncook.style.width = 10 + 'px';
            }
            if (cookcons != null && typeof cookcons !== "undefined") {
                if (cookcons.children[0] != null && typeof cookcons.children[0] !== "undefined") {
                    cookcons.children[0].style.display = "inline-block";
                    cookcons.children[0].style.width = 85 + '%';
                    cookcons.style.textAlign = "left";
                }
            }
            buttoncookie.style.top = prop1 + 'px';
        }
    }
});
$("#savetaskbutton").click(function (parms) {
    var obj = document.getElementsByClassName("textareainput")[0];
    var obj1 = document.getElementById("texthelp");
    if (typeof obj !== "undefined" && obj != null && typeof obj1 !== "undefined" && obj1 != null) {
        obj1.value = obj.value;
    }
});
$(".editfinishedtask").click(function (params) {
    EditFinishedTask("task");
});
function EditFinishedTask(ValueName) {
    var parentnotdiv = null;
    var notdiv = null;
    parentnotdiv = document.getElementsByClassName("parentnotificationdiv");

    if (parentnotdiv.length == 0) {
        parentnotdiv = document.createElement("div");
        parentnotdiv.classList.add("parentnotificationdiv");
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.prepend(parentnotdiv);
    }
    notdiv = document.createElement("div");
    notdiv.classList.add("notificationdiv");
    parentnotdiv.appendChild(notdiv);
    var titlenotdiv = document.createElement("div");
    titlenotdiv.classList.add("titlenotdiv");
    notdiv.appendChild(titlenotdiv);

    var titlespan = document.createElement("span");
    titlenotdiv.appendChild(titlespan);
    var titlespaname = "Edit ";
    for (var i = 0; i < ValueName.length; i++) {
        titlespaname += ValueName[i];
    }
    titlespan.innerHTML = titlespaname;
    titlespan.style.fontWeight = "bold";

    var questionparentdiv = document.createElement("div");
    questionparentdiv.classList.add("questionparentdiv");
    notdiv.appendChild(questionparentdiv);

    var questiondiv = document.createElement("div");
    questiondiv.classList.add("questiondiv");
    questionparentdiv.appendChild(questiondiv);

    var standardquestion = document.createElement("span");
    standardquestion.innerHTML = "You can't edit an already finished task!";
    standardquestion.style.fontWeight = "bold";
    standardquestion.classList.add("questiontext");
    questiondiv.appendChild(standardquestion);

    var parentnotdiv = document.createElement("div");
    parentnotdiv.classList.add("parentnotbuttondiv");
    notdiv.appendChild(parentnotdiv);

    var buttondiv = document.createElement("div");
    buttondiv.classList.add("buttondiv");
    buttondiv.style.width = 90 + '%';
    parentnotdiv.appendChild(buttondiv);

    var buttonformdiv = document.createElement("form");
    buttonformdiv.classList.add("buttondivform");
    if (ValueName != null) {
        var widt = $(window).width();
        if (ValueName == "task") {
            if (widt < 993) {
                buttonformdiv.action = "/Task/NewTaskMobile";
            }
            else {
                buttonformdiv.action = "/Task/NewTask";
            }
        }
    }
    buttonformdiv.method = "get";
    buttondiv.appendChild(buttonformdiv);
    var inputkeyelement = document.createElement("input");
    inputkeyelement.type = "hidden";
    buttonformdiv.appendChild(inputkeyelement);
    var buttonyes = document.createElement("button");
    buttonyes.type = "submit";
    buttonyes.classList.add("button");
    buttonyes.classList.add("btn-primary");
    buttonyes.classList.add("buttonyes");
    buttonyes.textContent = "Create new!";
    buttonformdiv.appendChild(buttonyes);
    var buttonformright = document.createElement("form");
    buttonformright.classList.add("buttondivformright");
    buttondiv.appendChild(buttonformright);
    var buttonno = document.createElement("a");
    buttonno.classList.add("buttonno");
    buttonno.classList.add("button");
    buttonno.classList.add("btn-danger");
    buttonno.textContent = "Close!";
    buttonno.style.paddingLeft = "10px";
    buttonno.style.paddingRight = "10px";
    buttonno.style.paddingTop = "4px";
    buttonno.style.paddingBottom = "3px";
    buttonno.addEventListener("click", CloseEditDiv);
    buttonformright.appendChild(buttonno);
}
function CloseEditDiv() {
    var element = this;
    var buttonformrightdiv = element.parentElement;
    buttonformrightdiv.removeChild(element);
    var buttondiv = buttonformrightdiv.parentElement;
    var buttondivform = buttondiv.children[0];
    var firstbuttondivform = buttondivform.children[0];
    buttondivform.removeChild(firstbuttondivform);
    buttondiv.removeChild(buttonformrightdiv);
    buttondiv.removeChild(buttondivform);
    var parentbuttondiv = buttondiv.parentElement;
    parentbuttondiv.removeChild(buttondiv);
    var notificationdivmain = parentbuttondiv.parentElement;
    notificationdivmain.removeChild(parentbuttondiv);
    var titlenotdiv = notificationdivmain.children[0];
    var titlespan = titlenotdiv.children[0];
    titlenotdiv.removeChild(titlespan);
    var questionparentdiv = notificationdivmain.children[1];
    var questiondiv = questionparentdiv.children[0];
    var firstspan = questiondiv.children[0];
    questiondiv.removeChild(firstspan);
    questionparentdiv.removeChild(questiondiv);
    notificationdivmain.removeChild(questionparentdiv);
    notificationdivmain.removeChild(titlenotdiv);
    var parentofparent = notificationdivmain.parentElement;
    parentofparent.removeChild(notificationdivmain);
    var list = document.getElementsByClassName("notificationdiv");
    if (list.length == 0) {
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.removeChild(parentofparent);
    }
}
var observer = new MutationObserver(function (mutations) {
    mutations.forEach(function (mutationRecord) {
        if (target != null && typeof target !== "undefined") {
            target.style.display = "none";
        }
        if (target1 != null && typeof target1 !== "undefined") {
            target1.style.display = "none";
        }
    });
});
var target = null;
target = document.getElementById("mainnotificationdiv");
var target1 = null;
target1 = document.getElementById("loadtest");
if (target != null && typeof target !== "undefined" && target1 != null && typeof target1 !== "undefined") {
    observer.observe(target, { attributes: true, attributeFilter: ['style'] });
    observer.observe(target1, { attributes: true, attributeFilter: ['style'] });
}
function MainDisplayNotification() {
    var TaskMainList = JSON.parse(sessionStorage.getItem("TaskList"));
    var AlarmMainList = JSON.parse(sessionStorage.getItem("AlarmList"));

    var TaskLenght = 0;
    var AlarmLenght = 0;
    if (TaskMainList != null) {
        TaskLenght = TaskMainList.length;
    }

    if (AlarmMainList != null) {
        AlarmLenght = AlarmMainList.length;
    }

    for (var i = 0; i < TaskLenght; i++) {
            if ((TaskMainList[i].Rated == "False" || TaskMainList[i].Rated == "false") && TaskMainList[i].Stats != "In processing!") {
                var ValueIsTimeToFinish = IsTimeToFinish(TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].EndTimeDate, TaskMainList[i].AcceptedNotification, i, "True", TaskMainList[i].Stats);
                if (ValueIsTimeToFinish.ReturnValue == true) {
                    var allnotification = document.getElementsByClassName("notificationdiv");
                    if (allnotification.length > 0) {
                        var any = false;
                        var testvalue = "Finish";
                        testvalue += TaskMainList[i].Key;

                        for (var j = 0; j < allnotification.length; j++) {
                            if (allnotification[j].children[2].children[0].children[0].children[3] != null && typeof allnotification[j].children[2].children[0].children[0].children[3] !== "undefined") {
                                if (allnotification[j].children[2].children[0].children[0].children[3].value == testvalue) {
                                    any = true;
                                }
                            }
                        }
                        if (any == false) {
                            CreateTaskNotification(ValueIsTimeToFinish.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToFinish.ReturnTimeValue, TaskMainList[i].EndTimeDate);

                            TaskMainList[i].AcceptedNotification = "False";
                            TaskMainList[i].Stats = "Finished!";
                            sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                            ClearAllExceptLast("3", TaskMainList[i].Key, "Start");
                            if (TaskMainList[i].Priority == "High") {
                                ThereIsAnyHigh = true;
                            }
                            ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                        }
                    }
                    else {
                        CreateTaskNotification(ValueIsTimeToFinish.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToFinish.ReturnTimeValue, TaskMainList[i].EndTimeDate);

                        TaskMainList[i].AcceptedNotification = "False";
                        TaskMainList[i].Stats = "Finished!";
                        sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                        ClearAllExceptLast("3", TaskMainList[i].Key, "Start");
                        if (TaskMainList[i].Priority == "High") {
                            ThereIsAnyHigh = true;
                        }
                        ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                    }
                }
            }
            if (TaskMainList[i].Stats == "In processing!" || (TaskMainList[i].Stats == "Finished!" && (TaskMainList[i].AcceptedNotification == "false" || TaskMainList[i].AcceptedNotification == "False"))) {
                var ValueIsTimeToFinish = IsTimeToFinish(TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].EndTimeDate, TaskMainList[i].AcceptedNotification, i, "False", TaskMainList[i].Stats);
                
                if (ValueIsTimeToFinish.ReturnValue == true) {

                    var allnotification = document.getElementsByClassName("notificationdiv");
                    if (allnotification.length > 0) {

                        var any = false;
                        var testvalue = "Finish";
                        testvalue += TaskMainList[i].Key;

                        for (var j = 0; j < allnotification.length; j++) {
                            if (allnotification[j].children[2].children[0].children[0].children[3] != null && typeof allnotification[j].children[2].children[0].children[0].children[3] !== "undefined") {
                                if (allnotification[j].children[2].children[0].children[0].children[3].value == testvalue) {
                                    any = true;
                                }
                            }
                        }
                        if (any == false) {

                            CreateTaskNotification(ValueIsTimeToFinish.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToFinish.ReturnTimeValue, TaskMainList[i].EndTimeDate);
                            TaskMainList[i].AcceptedNotification = "False";
                            TaskMainList[i].Stats = "Finished!";
                            sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                            ClearAllExceptLast("3", TaskMainList[i].Key, "Start");
                            if (TaskMainList[i].Priority == "High") {
                                ThereIsAnyHigh = true;
                            }
                            ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                        }
                    }
                    else {

                        CreateTaskNotification(ValueIsTimeToFinish.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToFinish.ReturnTimeValue, TaskMainList[i].EndTimeDate);
                        TaskMainList[i].AcceptedNotification = "False";
                        TaskMainList[i].Stats = "Finished!";
                        sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                        ClearAllExceptLast("3", TaskMainList[i].Key, "Start");
                        if (TaskMainList[i].Priority == "High") {
                            ThereIsAnyHigh = true;
                        }
                        ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                    }
                }
        }
            if (TaskMainList[i].Stats == "Upcoming!" || (TaskMainList[i].Stats == "In processing!" && (TaskMainList[i].AcceptedNotification == "false" || TaskMainList[i].AcceptedNotification == "False"))) {
                
                var ValueIsTimeToStart = IsTimeToStart(TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].EndTimeDate);
                
                if (ValueIsTimeToStart.ReturnValue == true && TaskMainList[i].AcceptedNotification == "False") {
                    
                    var allnotification = document.getElementsByClassName("notificationdiv");
                    if (allnotification.length > 0) {
                        var any = false;
                        var testvalue = "Start";
                        testvalue += TaskMainList[i].Key;

                        for (var j = 0; j < allnotification.length; j++) {
                            if (allnotification[j].children[2].children[0].children[0].children[3] != null && typeof allnotification[j].children[2].children[0].children[0].children[3] !== "undefined") {
                                if (allnotification[j].children[2].children[0].children[0].children[3].value == testvalue) {
                                    any = true;
                                }
                            }
                        }
                        if (any == false) {
                            CreateTaskNotification(ValueIsTimeToStart.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToStart.ReturnTimeValue, TaskMainList[i].EndTimeDate);

                            TaskMainList[i].AcceptedNotification = "False";
                            TaskMainList[i].Stats = "In processing!";
                            sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                            ClearAllExceptLast("2", TaskMainList[i].Key, "Task notification");
                            ClearAllExceptLast("3", TaskMainList[i].Key, "Task notification");
                            if (TaskMainList[i].Priority == "High") {
                                ThereIsAnyHigh = true;
                            }
                            ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                        }
                    }
                    else {
                        CreateTaskNotification(ValueIsTimeToStart.ReturnTextValue, TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, ValueIsTimeToStart.ReturnTimeValue, TaskMainList[i].EndTimeDate);

                        TaskMainList[i].AcceptedNotification = "False";
                        TaskMainList[i].Stats = "In processing!";
                        sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                        ClearAllExceptLast("2", TaskMainList[i].Key, "Task notification");
                        if (TaskMainList[i].Priority == "High") {
                            ThereIsAnyHigh = true;
                        }
                        ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                    }
                }
                
                if ((IsLessThan48Hours(TaskMainList[i].StartTimeDate, TaskMainList[i].EndTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days) == true && DifferenceBetweenNotifications(TaskMainList[i].NotificationEvery, TaskMainList[i].LastNotTimeDate) == true && TaskMainList[i].Stats == "Upcoming!")) {
                    if (TaskMainList[i].AcceptedNotification == "True" || TaskMainList[i].AcceptedNotification == "true") {

                        TaskMainList[i].AcceptedNotification = "False";
                        sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                    }
                    var NewLastNot = new Date();
                    TaskMainList[i].LastNotTimeDate = NewLastNot;
                    
                    sessionStorage.setItem('TaskList', JSON.stringify(TaskMainList));
                    if (TaskMainList[i].Priority == "Normal") {
                        DuringOfRingingTask[i] = 15;
                    }
                    if (TaskMainList[i].Priority == "Low") {
                        DuringOfRingingTask[i] = 5;
                    }
                    CreateTaskNotification("Task notification", TaskMainList[i].TaskName, TaskMainList[i].StartTimeDate, TaskMainList[i].CreatedFor, TaskMainList[i].Days, TaskMainList[i].Key, TaskMainList[i].SoundName, TaskMainList[i].Priority, i, 0, TaskMainList[i].EndTimeDate);
                    ClearAllExceptLast("1", TaskMainList[i].Key, "Task notification");
                    if (TaskMainList[i].Priority == "High") {
                        ThereIsAnyHigh = true;
                    }
                    ClearAllTimeOutExceptLast(i, ThereIsAnyHigh);
                }
            }
        }
    
    for (var j = 0; j < AlarmLenght; j++) {
            if (IsTimeForRing("Alarm ring", AlarmMainList[j].AlarmName, AlarmMainList[j].RingingTime, AlarmMainList[j].CreatedFor, AlarmMainList[j].Days, AlarmMainList[j].LastRinging)) {
                var any = false;
                var allnotification = document.getElementsByClassName("notificationdiv");

                if (allnotification == 0) {
                    any = false;
                }
                else {
                    for (var k = 0; k < allnotification.length; k++) {
                        if (allnotification[k].children[2].children[0].children[0].children[3] != null && typeof allnotification[k].children[2].children[0].children[0].children[3] !== "undefined") {
                            if (allnotification[k].children[2].children[0].children[0].children[3].value == "Finish" + AlarmMainList[j].Key) {
                                any = true;
                            }
                        }
                    }
                }

                if (any == false) {
                    AlarmMainList[j].Ringing = true;
                    CreateTaskNotification("Alarm ringing", AlarmMainList[j].AlarmName, AlarmMainList[j].RingingTime, AlarmMainList[j].CreatedFor, AlarmMainList[j].Days, AlarmMainList[j].Key, AlarmMainList[j].SoundName);
                }
            }
            if (IsTimeForSnooze("Alarm snooze", AlarmMainList[j].AlarmName, AlarmMainList[j].RingingTime, AlarmMainList[j].CreatedFor, AlarmMainList[j].Days, AlarmMainList[j].LastRinging, AlarmMainList[j].LastSnoozeDateTime) == true && AlarmMainList[j].Ringing == false) {
                var any = false;
                var allnotification = document.getElementsByClassName("notificationdiv");

                if (allnotification == 0) {
                    any = false;
                }
                else {
                    for (var k = 0; k < allnotification.length; k++) {
                        if (allnotification[k].children[2].children[0].children[0].children[3] != null && typeof allnotification[k].children[2].children[0].children[0].children[3] !== "undefined") {
                            if (allnotification[k].children[2].children[0].children[0].children[3].value == "Finish" + AlarmMainList[j].Key) {
                                any = true;
                            }
                        }
                    }
                }

                if (any == false) {
                    CreateTaskNotification("Alarm snooze", AlarmMainList[j].AlarmName, AlarmMainList[j].RingingTime, AlarmMainList[j].CreatedFor, AlarmMainList[j].Days, AlarmMainList[j].Key, AlarmMainList[j].SoundName, AlarmMainList[j].LastSnoozeDateTime);
                }
            }
        }
    
}
function IsTimeForRing(Title, AlarmName, StartDate, CreatedFor, Days, LastRinging) {
    var StartYear = parseInt(LastRinging[0].toString(1) + LastRinging[1].toString(1) + LastRinging[2].toString(1) + LastRinging[3].toString(1));
    var StartMonth = parseInt(LastRinging[5].toString(1) + LastRinging[6].toString(1));
    var StartDay = parseInt(LastRinging[8].toString(1) + LastRinging[9].toString(1));

    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));

    var CurrentTime = new Date();
    var CurrentTimeString = CurrentTime.toDateString();
    var CurrentDay = parseInt(CurrentTimeString[8].toString(1) + CurrentTimeString[9].toString(1));

    if (StartHour==24) {
        StartHour = 0;
    }

    if (CreatedFor == "1") {
        var CurrentTime = new Date();

        if (CurrentTime.getFullYear() == StartYear && (CurrentTime.getMonth() + 1) == StartMonth && CurrentTime.getDate() == StartDay) {
            return false;
        }

        if (CurrentTime.getHours() == StartHour && CurrentTime.getMinutes() == StartMinute) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";

        if (CurrentTime.getFullYear() == StartYear && (CurrentTime.getMonth() + 1) == StartMonth && CurrentDay == StartDay) {
            return false;
        }

        if (FindString(weekday[CurrentTime.getDay()], Days) == true) {
            if (CurrentTime.getHours() == StartHour && CurrentTime.getMinutes() == StartMinute) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
}
function IsTimeForSnooze(Title, AlarmName, StartDate, CreatedFor, Days, LastRinging,LastSnooze) {
    var StartYear = parseInt(LastRinging[0].toString(1) + LastRinging[1].toString(1) + LastRinging[2].toString(1) + LastRinging[3].toString(1));
    var StartMonth = parseInt(LastRinging[5].toString(1) + LastRinging[6].toString(1));
    var StartDay = parseInt(LastRinging[8].toString(1) + LastRinging[9].toString(1));

    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));
    
    var SnoozeHour = parseInt(LastSnooze[11].toString(1) + LastSnooze[12].toString(1)) + 1;
    var SnoozeMinute = parseInt(LastSnooze[14].toString(1) + LastSnooze[15].toString(1));

    var CurrentTime = new Date();
    var CurrentTimeString = CurrentTime.toDateString();
    var CurrentDay = parseInt(CurrentTimeString[8].toString(1) + CurrentTimeString[9].toString(1));
    if (StartHour == 24) {
        StartHour = 0;
    }
    if (CreatedFor == "1") {
        var CurrentTime1 = new Date();

        if (SnoozeHour == StartHour && SnoozeMinute == StartMinute) {
            return false;
        }

        if (CurrentTime1.getHours() == SnoozeHour && CurrentTime1.getMinutes() == SnoozeMinute) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        var CurrentTime1 = new Date();

        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";

        if (FindString(weekday[CurrentTime.getDay()], Days) == true) {
            if (SnoozeHour == StartHour && SnoozeMinute == StartMinute) {
                return false;
            }
        }
        else {
            return false;
        }

        if (FindString(weekday[CurrentTime.getDay()], Days) == true) {
            if (CurrentTime1.getHours() == SnoozeHour && CurrentTime1.getMinutes() == SnoozeMinute) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }
}
function PlaySoundNotification(SoundName, Extension, Index, Priority) {
    if (Priority != "High" && DuringOfRingingTask[Index] != 0) {
        var audiointo = new Audio();
        var root = "../../Sounds/" + SoundName + Extension;
        audiointo.src = root;
        audiointo.play();
        DuringOfRingingTask[Index] += -1;
    }
    else {
        if (DuringOfRingingTask[Index] == 0) {
            clearTimeout(TaskInterval[Index]);
        }
    }
    if (Priority == "High") {
        var audiointo = new Audio();
        var root = "../../Sounds/" + SoundName + Extension;
        audiointo.src = root;
        audiointo.play();
    }
}
function PlayAlarmRinging(SoundName, Extension) {
    var root = "../../Sounds/" + SoundName + Extension;
    var audiointo = new Audio();
    audiointo.src = root;
    audiointo.play();
}
function CreateTaskNotification(Title, TaskName, StartTime, CreatedFor, Days, Key, SoundName, Priority, Index, TimeInSeconds, EndTime) {
   
    var parentnotdiv = null;
    var notdiv = null;
    parentnotdiv = document.getElementsByClassName("parentnotificationdiv");

    if (parentnotdiv.length == 0) {
        parentnotdiv = document.createElement("div");
        parentnotdiv.classList.add("parentnotificationdiv");
        var firstelement = document.getElementById("topbar");
        var mainunderbody = document.getElementById("mainunderbody");
        mainunderbody.insertBefore(parentnotdiv, firstelement);
    }
    else {
        parentnotdiv = document.getElementsByClassName("parentnotificationdiv")[0];
    }
    notdiv = document.createElement("div");
    notdiv.classList.add("notificationdiv");
    parentnotdiv.appendChild(notdiv);
    var titlenotdiv = document.createElement("div");
    titlenotdiv.classList.add("titlenotdiv");
    notdiv.appendChild(titlenotdiv);

    var titlespan = document.createElement("span");
    titlenotdiv.appendChild(titlespan);

    if (Title.toString() == "Started") {
        titlespan.innerHTML = "Task started";
    }

    if (Title.toString() == "Starting") {
        titlespan.innerHTML = "Task starting";
    }

    if (Title.toString() == "Finished") {
        titlespan.innerHTML = "Task finished";
    }

    if (Title.toString() == "Finishing") {
        titlespan.innerHTML = "Task finishing";
    }

    if (Title.toString() != "Started" && Title.toString() != "Starting" && Title.toString() != "Finished" && Title.toString() != "Finishing") {
        titlespan.innerHTML = Title;
    }

    titlespan.style.fontWeight = "bold";

    var questionparentdiv = document.createElement("div");
    questionparentdiv.classList.add("questionparentdiv");
    notdiv.appendChild(questionparentdiv);

    var questiondiv = document.createElement("div");
    questiondiv.classList.add("questiondiv");
    questionparentdiv.appendChild(questiondiv);

    var standardquestion = document.createElement("span");
    var tasknamelenght = TaskName;
    var temp = TaskName;

    if (TaskName.length > 35) {
        temp = "";
        for (var i = 0; i < 36; i++) {
            temp += TaskName[i];
        }
        temp += "...";
    }
    if (Title.toString() != "Started" && Title.toString() != "Starting" && Title.toString() != "Finished" && Title.toString() != "Finishing" && Title.toString() != "Alarm ringing" && Title.toString() != "Alarm snooze") {
        TaskName = "Your ";
        TaskName += temp;
        TaskName += " ";
        TaskName += "task will start in ";
        TaskName += GetNotificationHoursMinutes(StartTime, EndTime, CreatedFor, Days, true);
        TaskName += "!";
        standardquestion.innerHTML = TaskName;
    }

    if (Title.toString() == "Started" || Title.toString() == "Finished") {
        TaskName = "Your ";
        TaskName += temp;
        TaskName += " ";
        if (Title.toString() == "Started") {
            TaskName += "task started ";
        }
        else {
            TaskName += "task finished ";
        }

        TaskName += GetStartedTime(TimeInSeconds);
        TaskName += " ago!";
        standardquestion.innerHTML = TaskName;
    }

    if (Title.toString() == "Starting" || Title.toString() == "Finishing" ) {
        TaskName = "Your ";
        TaskName += temp;
        TaskName += " ";
        if (Title.toString() == "Starting") {
            TaskName += "task is starting!";
        }
        else {
            TaskName += "task is finishing!";
        }
        standardquestion.innerHTML = TaskName;
    }

    if (Title.toString() == "Alarm ringing" || Title.toString() == "Alarm snooze") {
        temp += "!";
        standardquestion.innerHTML = temp;
    }

    standardquestion.style.fontWeight = "bold";
    var windowwidth = $(window).width();
    if (windowwidth < 501) {
        standardquestion.style.fontSize = 12 + 'px';
    }
    else {
        standardquestion.style.fontSize = 16 + 'px';
    }
    standardquestion.classList.add("questiontext");
    questiondiv.appendChild(standardquestion);

    var parentnotdiv = document.createElement("div");
    parentnotdiv.classList.add("parentnotbuttondiv");
    notdiv.appendChild(parentnotdiv);

    var buttondiv = document.createElement("div");
    buttondiv.classList.add("buttondiv");
    parentnotdiv.appendChild(buttondiv);

    var heightofparentdiv = questionparentdiv.offsetHeight;
    var heightofquestiondiv = questiondiv.offsetHeight;
    var padd = 0;
    if (heightofquestiondiv > 25) {
        if (windowwidth < 501) {
            padd = heightofparentdiv - (heightofquestiondiv * 2) - 13;
        }
        else {
            padd = heightofparentdiv - (heightofquestiondiv * 2) - 5;
        }
    }
    else {
        padd = heightofparentdiv - (heightofquestiondiv * 2) - 42;
    }
    questionparentdiv.style.paddingTop = padd + 'px';

    if (Title == "Task notification" || Title == "Started" || Title == "Starting") {
        var buttonformdiv = document.createElement("form");
        buttonformdiv.classList.add("buttondivform");
        buttonformdiv.action = "/Task/SetLastNotification";
        buttonformdiv.method = "post";
        buttondiv.appendChild(buttonformdiv);
        var inputkeyelement = document.createElement("input");
        inputkeyelement.type = "hidden";
        inputkeyelement.name = "Key";
        inputkeyelement.value = Key;
        buttonformdiv.appendChild(inputkeyelement);
        var inputkeyelement1 = document.createElement("input");
        inputkeyelement1.type = "hidden";
        inputkeyelement1.name = "Value";
        inputkeyelement1.value = Title;
        buttonformdiv.appendChild(inputkeyelement1);
        var buttonyes = document.createElement("button");
        buttonyes.type = "submit";
        buttonyes.classList.add("button");
        buttonyes.classList.add("btn-primary");
        buttonyes.classList.add("buttonyes");
        buttonyes.textContent = "Ok!";
        buttonformdiv.appendChild(buttonyes);
        if (Title == "Started" || Title == "Starting") {
            var inputkeyelement2 = document.createElement("input");
            inputkeyelement2.type = "hidden";
            inputkeyelement2.name = "StartValue";
            inputkeyelement2.value = "Start" + Key;
            buttonformdiv.appendChild(inputkeyelement2);
        }
    }
    else {
        if (Title != "Alarm ringing" && Title != "Alarm snooze") {
            buttondiv.style.width = 70 + '%';
            var buttonformdiv = document.createElement("form");
            buttonformdiv.classList.add("buttondivform");
            buttonformdiv.action = "/Task/SetLastNotification";
            buttonformdiv.method = "post";
            buttondiv.appendChild(buttonformdiv);
            var inputkeyelement = document.createElement("input");
            inputkeyelement.type = "hidden";
            inputkeyelement.name = "Key";
            inputkeyelement.value = Key;
            buttonformdiv.appendChild(inputkeyelement);
            var inputkeyelement1 = document.createElement("input");
            inputkeyelement1.type = "hidden";
            inputkeyelement1.name = "Value";
            inputkeyelement1.value = Title;
            buttonformdiv.appendChild(inputkeyelement1);
            var buttonyes = document.createElement("button");
            buttonyes.type = "submit";
            buttonyes.classList.add("button");
            buttonyes.classList.add("btn-success");
            buttonyes.classList.add("buttonyes");
            buttonyes.textContent = "Successfully!";
            buttonformdiv.appendChild(buttonyes);
            var inputkeyelement2 = document.createElement("input");
            inputkeyelement2.type = "hidden";
            inputkeyelement2.name = "FinishValue";
            inputkeyelement2.value = "Finish" + Key;
            buttonformdiv.appendChild(inputkeyelement2);

            var success = document.createElement("input");
            success.type = "hidden";
            success.name = "Success";
            success.value = "true";
            buttonformdiv.appendChild(success);

            var buttonformdiv1 = document.createElement("form");
            buttonformdiv1.classList.add("buttondivform");
            buttonformdiv1.action = "/Task/SetLastNotification";
            buttonformdiv1.method = "post";
            buttonformdiv1.classList.add("buttondivformright");
            buttondiv.appendChild(buttonformdiv1);
            var inputkeyelement = document.createElement("input");
            inputkeyelement.type = "hidden";
            inputkeyelement.name = "Key";
            inputkeyelement.value = Key;
            buttonformdiv1.appendChild(inputkeyelement);
            var inputkeyelement1 = document.createElement("input");
            inputkeyelement1.type = "hidden";
            inputkeyelement1.name = "Value";
            inputkeyelement1.value = Title;
            buttonformdiv1.appendChild(inputkeyelement1);
            var buttonno = document.createElement("button");
            buttonno.type = "submit";
            buttonno.classList.add("button");
            buttonno.classList.add("btn-danger");
            buttonno.classList.add("buttonno");
            buttonno.textContent = "Unsuccessfully!";
            buttonformdiv1.appendChild(buttonno);

            var unsuccess = document.createElement("input");
            unsuccess.type = "hidden";
            unsuccess.name = "Success";
            unsuccess.value = "false";
            buttonformdiv1.appendChild(unsuccess);
        }
        else {
            buttondiv.style.width = 70 + '%';
            var buttonformdiv = document.createElement("form");
            buttonformdiv.classList.add("buttondivform");
            buttonformdiv.action = "/Alarm/SetLastAlarm";
            buttonformdiv.method = "post";
            buttondiv.appendChild(buttonformdiv);
            var inputkeyelement = document.createElement("input");
            inputkeyelement.type = "hidden";
            inputkeyelement.name = "Key";
            inputkeyelement.value = Key;
            buttonformdiv.appendChild(inputkeyelement);
            var inputkeyelement1 = document.createElement("input");
            inputkeyelement1.type = "hidden";
            inputkeyelement1.name = "Value";
            inputkeyelement1.value = Title;
            buttonformdiv.appendChild(inputkeyelement1);
            var buttonyes = document.createElement("button");
            buttonyes.type = "submit";
            buttonyes.classList.add("button");
            buttonyes.classList.add("btn-danger");
            buttonyes.classList.add("buttonyes");
            buttonyes.textContent = "Close!";
            buttonformdiv.appendChild(buttonyes);
            var inputkeyelement2 = document.createElement("input");
            inputkeyelement2.type = "hidden";
            inputkeyelement2.name = "FinishValue";
            inputkeyelement2.value = "Finish" + Key;
            buttonformdiv.appendChild(inputkeyelement2);

            var success = document.createElement("input");
            success.type = "hidden";
            success.name = "Success";
            success.value = "Close";
            buttonformdiv.appendChild(success);

            var buttonformdiv1 = document.createElement("form");
            buttonformdiv1.classList.add("buttondivform");
            buttonformdiv1.action = "/Alarm/SetLastAlarm";
            buttonformdiv1.method = "post";
            buttonformdiv1.classList.add("buttondivformright");
            buttondiv.appendChild(buttonformdiv1);
            var inputkeyelement = document.createElement("input");
            inputkeyelement.type = "hidden";
            inputkeyelement.name = "Key";
            inputkeyelement.value = Key;
            buttonformdiv1.appendChild(inputkeyelement);
            var inputkeyelement1 = document.createElement("input");
            inputkeyelement1.type = "hidden";
            inputkeyelement1.name = "Value";
            inputkeyelement1.value = "Snooze";
            buttonformdiv1.appendChild(inputkeyelement1);
            var buttonno = document.createElement("button");
            buttonno.type = "submit";
            buttonno.classList.add("button");
            buttonno.classList.add("btn-primary");
            buttonno.classList.add("buttonno");
            buttonno.textContent = "Snooze!";
            buttonformdiv1.appendChild(buttonno);

            var unsuccess = document.createElement("input");
            unsuccess.type = "hidden";
            unsuccess.name = "Success";
            unsuccess.value = "Snooze";
            buttonformdiv1.appendChild(unsuccess);
        }
    }

    if (Title != "Alarm ringing" && Title != "Alarm snooze") {
        if (Priority == "High") {
            clearTimeout(HighInterval);
        }
        else {
            clearTimeout(TaskInterval[Index]);
        }

        PlaySoundNotification(SoundName, ".mp3", Index, Priority);

        if (Priority == "High") {

            if (SoundName == "TaskSound1" || SoundName == "TaskSound3" || SoundName == "TaskSound4") {
                clearTimeout(HighInterval);
                HighInterval = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 1500);
            }
            if (SoundName == "TaskSound2" || SoundName == "TaskSound6") {
                clearTimeout(HighInterval);
                HighInterval = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 1800);
            }
            if (SoundName == "TaskSound5") {
                clearTimeout(HighInterval);
                HighInterval = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 2000);
            }
            if (SoundName == "TaskSound7") {
                clearTimeout(HighInterval);
                HighInterval = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 2500);
            }
        }

        if (Priority != "High") {
            if (SoundName == "TaskSound1" || SoundName == "TaskSound3" || SoundName == "TaskSound4") {
                clearTimeout(TaskInterval[Index]);
                TaskInterval[Index] = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 1500);
            }
            if (SoundName == "TaskSound2" || SoundName == "TaskSound6") {
                clearTimeout(TaskInterval[Index]);
                TaskInterval[Index] = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 1800);
            }
            if (SoundName == "TaskSound5") {
                clearTimeout(TaskInterval[Index]);
                TaskInterval[Index] = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 2000);
            }
            if (SoundName == "TaskSound7") {
                clearTimeout(TaskInterval[Index]);
                TaskInterval[Index] = setInterval(function () { PlaySoundNotification(SoundName, ".mp3", Index, Priority); }, 2500);
            }
        }
    }
    else {
        if (SoundName != "AlarmSound5" && SoundName != "AlarmSound6") {
            PlayAlarmRinging(SoundName, ".mp3");
        }
        else {
            PlayAlarmRinging(SoundName, ".wav");
        }

        if (SoundName == "AlarmSound1") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".mp3"); }, 3500);
        }
        if (SoundName == "AlarmSound2") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".mp3"); }, 7500);
        }
        if (SoundName == "AlarmSound3") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".mp3"); }, 6000);
        }
        if (SoundName == "AlarmSound4") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".mp3"); }, 20000);
        }
        if (SoundName == "AlarmSound5") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".wav"); }, 10200);
        }
        if (SoundName == "AlarmSound6") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".wav"); }, 15000);
        }
        if (SoundName == "AlarmSound7") {
            AlarmInterval[Index] = setInterval(function () { PlayAlarmRinging(SoundName, ".mp3"); }, 5000);
        }
    }
}
function GetNotificationHoursMinutes(StartDate, EndDate, CreatedFor, Days, AcceptNot) {
    var finalanswer = "";

    var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
    var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1))-1;
    var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));

    var StartTimeConvert = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);
   

    if (CreatedFor == "1") {
        var CurrentTime = new Date();

        const diffTime = Math.abs(CurrentTime - StartTimeConvert);
        const diffminute = Math.ceil(diffTime / (1000 * 60));
        
        if (diffminute > 59) {
            var hours = Math.round(diffminute / 60);
            var minute = diffminute - (hours * 60);


            if (minute < 0) {
                hours += -1;
            }

            minute = diffminute - (hours * 60);

            if (minute > 0) {
                if (hours > 1) {
                    finalanswer = hours.toString();
                    finalanswer += " hours and ";
                    finalanswer += minute.toString();

                    if (minute > 1) {
                        finalanswer += " minutes!";
                    }
                    else {
                        finalanswer += " minute!";
                    }

                }
                else {
                    finalanswer = "1 hour and "
                    finalanswer += minute.toString();
                    if (minute > 1) {
                        finalanswer += " minutes!";
                    }
                    else {
                        finalanswer += " minute!";
                    }
                }
            }
            else {
                if (hours > 1) {
                    finalanswer = hours.toString();
                    finalanswer += " hours!";
                }
                else {
                    finalanswer = "1 hour!"
                }
            }
        }
        else {
            finalanswer += diffminute.toString();

            if (diffminute > 1) {
                finalanswer += " minutes!";
            }
            else {
                finalanswer += " minute!";
            }
        }
    }
    else {
        var CurrentTime = new Date();

        var numberofdays = 0;
        CurrentTime.setDate(CurrentTime.getDay() + 1);

        var ReturnedObj = GetActiveDayAndNumberOfDays(StartDate, EndDate, Days, AcceptNot,"Start");
        numberofdays = ReturnedObj.NumberOfDays;

        var CurrentTime1 = new Date();

        if (StartHour < 24) {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays);
        }
        else {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays-1);
        }
        
        CurrentTime1.setHours(StartHour);
        CurrentTime1.setMinutes(StartMinute);


        var RealCurrentTime = new Date();
        
        const diffTime = Math.abs(RealCurrentTime - CurrentTime1);
        const diffminute = Math.ceil(diffTime / (1000 * 60));

        if (diffminute > 59) {
            var hours = Math.round(diffminute / 60);
            var minute = diffminute - (hours * 60);

            if (minute < 0) {
                hours += -1;
            }

            minute = diffminute - (hours * 60);

            if (minute > 0) {
                if (hours > 1) {
                    finalanswer = hours.toString();
                    finalanswer += " hours and ";
                    finalanswer += minute.toString();

                    if (minute > 1) {
                        finalanswer += " minutes!";
                    }
                    else {
                        finalanswer += " minute!";
                    }

                }
                else {
                    finalanswer = "1 hour and "
                    finalanswer += minute.toString();
                    if (minute > 1) {
                        finalanswer += " minutes!";
                    }
                    else {
                        finalanswer += " minute!";
                    }
                }
            }
            else {
                if (hours > 1) {
                    finalanswer = hours.toString();
                    finalanswer += " hours!";
                }
                else {
                    finalanswer = "1 hour!"
                }
            }
        }
        else {
            var roundminute = Math.round(diffminute);
            finalanswer += roundminute.toString();

            if (roundminute > 1) {
                finalanswer += " minutes";
            }
            else {
                finalanswer += " minute";
            }
        }
    }

    return finalanswer;
}
function GetActiveDayAndNumberOfDays(StartDate, EndDate, Days, AcceptedNotification,Valuee,Stats) {
    var numberofdays = 0;
    var CurrentTime = new Date();

    var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
    var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1)) - 1;
    var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));

    var RealStartStart = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);


    var EndYear = parseInt(EndDate[0].toString(1) + EndDate[1].toString(1) + EndDate[2].toString(1) + EndDate[3].toString(1));
    var EndMonth = parseInt(EndDate[5].toString(1) + EndDate[6].toString(1)) - 1;
    var EndDay = parseInt(EndDate[8].toString(1) + EndDate[9].toString(1));
    var EndHour = parseInt(EndDate[11].toString(1) + EndDate[12].toString(1)) + 1;
    var EndMinute = parseInt(EndDate[14].toString(1) + EndDate[15].toString(1));

    var RealEndEnd = new Date(EndYear, EndMonth, EndDay, EndHour, EndMinute);

    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";

    var ActiveDay = "";
    var firsttime = 0;
    var IsLower = 0;
    var ActiveBefore = 0;

    if (RealEndEnd < RealStartStart) {
        IsLower = 1;
    }

    
    var IsHigher = 0;

    

    if (Valuee == "Finish") {
        while (ActiveDay == "") {
            var TempDate = new Date(CurrentTime);

            var CurretnDay = weekday[TempDate.getDay()];
            TempDate.setDate(TempDate.getDate() - 1);

            var CurretnDay1 = weekday[TempDate.getDay()];
            TempDate.setDate(TempDate.getDate() - 1);

            var CurretnDay2 = weekday[TempDate.getDay()];

            if ((FindString(CurretnDay1, Days) == true && FindString(CurretnDay, Days) == true) == true || (FindString(CurretnDay1, Days) == true && FindString(CurretnDay, Days) == false) == true) {
                ActiveBefore = 1;
            }
            else {
                if (FindString(CurretnDay1, Days) == true && FindString(CurretnDay2, Days) == true) {
                    ActiveBefore = 1;
                }
            }
            if (FindString(weekday[CurrentTime.getDay()], Days) == true || (ActiveBefore==1 && IsLower==1)) {
                if (firsttime == 0) {
                    firsttime++;
                    var current2 = new Date();
                    
                    var current2tostring = current2.toDateString();

                    var CurrentDay = parseInt(current2tostring[8].toString(1) + current2tostring[9].toString(1));

                    var RealEnd = new Date(current2.getFullYear(), current2.getMonth(), CurrentDay, RealEndEnd.getHours(), RealEndEnd.getMinutes(),0);
                    var RealStart = new Date(current2.getFullYear(), current2.getMonth(), CurrentDay, RealStartStart.getHours(), RealStartStart.getMinutes(),0);

                    
                    if (IsLower == 1 && ActiveBefore == 1) {
                        RealStart.setDate(RealStart.getDate() - 1);
                        IsHigher = 1;
                        ActiveDay = weekday[CurrentTime.getDay()];
                    }
                    
                    if (IsLower == 1 && RealEnd <= current2) {
                        ActiveDay = weekday[CurrentTime.getDay()];
                        if (IsHigher == 0) {
                            numberofdays++;
                            RealEnd.setDate(RealEnd.getDate() + 1);
                        }
                    }
                    if (IsLower != 1) {
                        if (RealEnd >= current2) {
                            ActiveDay = weekday[CurrentTime.getDay()];
                        }
                    }
                }
                else {
                    if (IsLower == 1) {
                        numberofdays++;
                        ActiveDay = weekday[CurrentTime.getDay()];
                    }
                    else {
                        ActiveDay = weekday[CurrentTime.getDay()];
                    }
                    CurrentTime.setDate(CurrentTime.getDate() + 1);
                }

            }
            else {
                CurrentTime.setDate(CurrentTime.getDate() + 1);
                numberofdays++;
                firsttime++;
            }
        }
    }
    else {
        while (ActiveDay == "") {
            var TempDate = new Date(CurrentTime);
            var CurretnDay = weekday[TempDate.getDay()];
            TempDate.setDate(TempDate.getDate() - 1);
            var CurretnDay1 = weekday[TempDate.getDay()];
            TempDate.setDate(TempDate.getDate() - 1);
            var CurretnDay2 = weekday[TempDate.getDay()];

            if ((FindString(CurretnDay1, Days) == true && FindString(CurretnDay, Days) == true) || (FindString(CurretnDay1, Days) == true && FindString(CurretnDay, Days) == false)) {
                ActiveBefore = 1;
            }
            else {
                if (FindString(CurretnDay1, Days) == true && FindString(CurretnDay2, Days) == true) {
                    ActiveBefore = 1;
                }
            }

            if (FindString(weekday[CurrentTime.getDay()], Days) == true || (ActiveBefore == 1 && IsLower == 1)) {
                if (firsttime == 0) {
                    firsttime++;
                    var current2 = new Date();
                    
                    var current2tostring = current2.toDateString();

                    var CurrentDay = parseInt(current2tostring[8].toString(1) + current2tostring[9].toString(1));

                    var RealEnd = new Date(current2.getFullYear(), current2.getMonth(), CurrentDay, RealEndEnd.getHours(), RealEndEnd.getMinutes(), 0);
                    var RealStart = new Date(current2.getFullYear(), current2.getMonth(), CurrentDay, RealStartStart.getHours(), RealStartStart.getMinutes(), 0);

                    var Open = false;
                    if (IsLower == 1 && ActiveBefore == 1 && RealEnd > current2) {
                        RealStart.setDate(RealStart.getDate() - 1);
                        numberofdays--;
                        ActiveDay = weekday[CurrentTime.getDay()];
                        Open = true;
                    }
                    
                    if (IsLower == 1 && ActiveBefore == 1 && RealEnd < current2 && Open == false) {
                        if (Open == true) {
                            RealStart.setDate(RealStart.getDate() + 1);
                            numberofdays++;
                        }
                        IsHigher = false;
                    }
                    else {
                        if (Open == false&&IsLower==1) {
                            if (IsLower == 1 && FindString(weekday[CurrentTime.getDay()], Days) == true) {
                                ActiveDay = weekday[CurrentTime.getDay()];
                            }
                            else {
                                CurrentTime.setDate(CurrentTime.getDate() + 1);
                                numberofdays++;
                            }
                        }
                    }
                    
                    if (IsLower != 1) {
                        if (RealStart > current2) {
                            ActiveDay = weekday[CurrentTime.getDay()];
                        }
                        else {
                            if (RealStart <= current2 && RealEnd > current2) {
                                ActiveDay = weekday[CurrentTime.getDay()];
                            }
                            else {
                                CurrentTime.setDate(CurrentTime.getDate() + 1);
                                numberofdays++;
                            }
                        }
                        
                    }
                    
                }
                else {
                    if (IsLower != true) {
                        ActiveDay = weekday[CurrentTime.getDay()];
                    }
                    else {
                        if (FindString(weekday[CurrentTime.getDay()], Days) == true) {   
                            ActiveDay = weekday[CurrentTime.getDay()];
                        }
                        else {
                            CurrentTime.setDate(CurrentTime.getDate() + 1);
                            numberofdays++;
                        }
                    }
                }

            }
            else {
                CurrentTime.setDate(CurrentTime.getDate() + 1);
                numberofdays++;
                firsttime++;
            }
        }
    }
    
    var ObjectForReturn = new Object();
    ObjectForReturn.ActiveDay = ActiveDay;
    ObjectForReturn.NumberOfDays = numberofdays;
    
    return ObjectForReturn;
}
function IsLessThan48Hours(StartDate, EndDate, CreatedFor, Days) {
    var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
    var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1)) - 1;
    var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));

    var StartTimeConvert = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);

    if (CreatedFor == "1") {
        var CurrentTime = new Date();

        const diffTime = Math.abs(CurrentTime - StartTimeConvert);
        const diffminute = Math.ceil(diffTime / (1000 * 60));

        if (diffminute < 2880) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        var numberofdays = 0;
        var CurrentTime = new Date();
        CurrentTime.setDate(CurrentTime.getDay() + 1);

        var ActiveDay = "";

        var ReturnedObject = GetActiveDayAndNumberOfDays(StartDate, EndDate, Days, true,"Start");

        numberofdays = ReturnedObject.NumberOfDays;
        ActiveDay = ReturnedObject.ActiveDay;
        var CurrentTime1 = new Date();

        if (StartHour < 24) {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays);
        }
        else {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays - 1);
        }

        CurrentTime1.setHours(StartHour);
        CurrentTime1.setMinutes(StartMinute);

        var RealCurrentTime = new Date();

        const diffTime = Math.abs(RealCurrentTime - CurrentTime1);
        const diffminute = Math.ceil(diffTime / (1000 * 60));
        
        if (diffminute < 2880) {
            return true;
        }
        else {
            return false;
        }
    }
}
function DifferenceBetweenNotifications(NotificationEvery, StartDate) {
    
    var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
    var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1)) - 1;
    var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + CurrentOffSet+1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1)) + CurrentOffSetMinute;

    var StartTimeConvert = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);
    
    var CurrentTime = new Date();

    const diffTime = Math.abs(CurrentTime - StartTimeConvert);
    const diffminute = Math.ceil(diffTime / (1000 * 60));
    if (diffminute > NotificationEvery) {
        return true;
    }
    else {
        return false;
    }
}
function ClearAllExceptLast(Which, Key, Value) {
    var allnotification = document.getElementsByClassName("notificationdiv");

    for (var i = 0; i < allnotification.length; i++) {
        if (i != allnotification.length - 1&&allnotification[i].children[0].children[0].innerText!="Allow sounds") {
            DeleteNotification(i);
        }
    }
}
function DeleteNotification(Index) {
    var allnotification = document.getElementsByClassName("notificationdiv");

    var element = allnotification[Index];
    var parentelement = element.parentElement;
    parentelement.removeChild(element);
}
function IsTimeToStart(StartDate, CreatedFor, Days, EndDate) {
    var StartYear = parseInt(StartDate[0].toString(1) + StartDate[1].toString(1) + StartDate[2].toString(1) + StartDate[3].toString(1));
    var StartMonth = parseInt(StartDate[5].toString(1) + StartDate[6].toString(1)) - 1;
    var StartDay = parseInt(StartDate[8].toString(1) + StartDate[9].toString(1));
    var StartHour = parseInt(StartDate[11].toString(1) + StartDate[12].toString(1)) + 1;
    var StartMinute = parseInt(StartDate[14].toString(1) + StartDate[15].toString(1));

    var StartTimeConvert = new Date(StartYear, StartMonth, StartDay, StartHour, StartMinute);

    var CurrentTime = new Date();

    if (CreatedFor == "1") {
        if (CurrentTime.getFullYear() == StartTimeConvert.getFullYear() && CurrentTime.getMonth() == StartTimeConvert.getMonth() && CurrentTime.getDay() == StartTimeConvert.getDay() && CurrentTime.getHours() == StartTimeConvert.getHours() && CurrentTime.getMinutes() == StartTimeConvert.getMinutes()) {
            var ReturnObject = new Object();
            ReturnObject.ReturnValue = true;
            ReturnObject.ReturnTextValue = "Starting";
            ReturnObject.ReturnTimeValue = 0;
            return ReturnObject;
        }
        if (CurrentTime > StartTimeConvert) {
            const diffTime = Math.abs(CurrentTime - StartTimeConvert);
            const diffseconds = Math.ceil(diffTime / (1000));
            if (diffseconds < 60) {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Starting";
                ReturnObject.ReturnTimeValue = 0;
                return ReturnObject;
            }
            else {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Started";
                ReturnObject.ReturnTimeValue = diffseconds;
                return ReturnObject;
            }
        }

        var ReturnObject = new Object();
        ReturnObject.ReturnValue = false;
        ReturnObject.ReturnTextValue = "No!";
        ReturnObject.ReturnTimeValue = 0;
        return ReturnObject;
    }
    else {
        var numberofdays = 0;
        var CurrentTime = new Date();

        var ReturnedObj = GetActiveDayAndNumberOfDays(StartDate, EndDate, Days, true,"Start");
        numberofdays = ReturnedObj.NumberOfDays;
        var CurrentTime1 = new Date();

        
        if (StartHour < 24) {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays);
        }
        else {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays - 1);
        }

        CurrentTime1.setHours(StartHour);
        CurrentTime1.setMinutes(StartMinute);
        
        var RealCurrentTime = new Date();

        if (RealCurrentTime.getFullYear() == CurrentTime1.getFullYear() && RealCurrentTime.getMonth() == CurrentTime1.getMonth() && RealCurrentTime.getDate() == CurrentTime1.getDate() && RealCurrentTime.getHours() == CurrentTime1.getHours() && RealCurrentTime.getMinutes() == CurrentTime1.getMinutes()) {
            
            var ReturnObject = new Object();
            ReturnObject.ReturnValue = true;
            ReturnObject.ReturnTextValue = "Starting";
            ReturnObject.ReturnTimeValue = 0;
            return ReturnObject;
        }
        if (RealCurrentTime > CurrentTime1) {

            const diffTime = Math.abs(RealCurrentTime - CurrentTime1);
            const diffseconds = Math.ceil(diffTime / (1000));

            if (diffseconds < 60) {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Starting";
                ReturnObject.ReturnTimeValue = 0;
                return ReturnObject;
            }
            else {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Started";
                ReturnObject.ReturnTimeValue = diffseconds;
                return ReturnObject;
            }
        }
        var ReturnObject = new Object();
        ReturnObject.ReturnValue = false;
        ReturnObject.ReturnTextValue = "No!";
        ReturnObject.ReturnTimeValue = 0;
        return ReturnObject;

    }
}
function FindString(substr, str) {
    
    var numberofsame = 0;
    var index = 0;
    for (var i = 0; i < str.length; i++) {
        if (str[i] == substr[index]) {
            numberofsame++;
            index++;
            if (numberofsame == substr.length)
                return true;
        }
        else {
            numberofsame = 0;
            index = 0;
        }
    }

    return false;
}
function GetStartedTime(TimeInSeconds) {
    var finalanswer = "";
    var diffminute = TimeInSeconds / 60;
    if (diffminute > 59) {
        var hours = Math.round(diffminute / 60);
        var minute = Math.round(diffminute - (hours * 60));
        if (minute < 0) {
            hours += -1;
        }

        minute = Math.round(diffminute - (hours * 60));

        if (minute > 0) {
            if (hours > 1) {
                finalanswer = hours.toString();
                finalanswer += " hours and ";
                finalanswer += minute.toString();

                if (minute > 1) {
                    finalanswer += " minutes";
                }
                else {
                    finalanswer += " minute";
                }

            }
            else {
                finalanswer = "1 hour and "
                finalanswer += minute.toString();
                if (minute > 1) {
                    finalanswer += " minutes";
                }
                else {
                    finalanswer += " minute";
                }
            }
        }
        else {
            if (hours > 1) {
                finalanswer = hours.toString();
                finalanswer += " hours";
            }
            else {
                finalanswer = "1 hour"
            }
        }
    }
    else {

        var roundminute = Math.round(diffminute);
        finalanswer += roundminute.toString();

        if (roundminute > 1) {
            finalanswer += " minutes";
        }
        else {
            finalanswer += " minute";
        }
    }
    return finalanswer;
}
function IsTimeToFinish(StartDate, CreatedFor, Days, EndDate, AcceptedNotification,i,Rated,Stats) {
    var EndYear = parseInt(EndDate[0].toString(1) + EndDate[1].toString(1) + EndDate[2].toString(1) + EndDate[3].toString(1));
    var EndMonth = parseInt(EndDate[5].toString(1) + EndDate[6].toString(1)) - 1;
    var EndDay = parseInt(EndDate[8].toString(1) + EndDate[9].toString(1));
    var EndHour = parseInt(EndDate[11].toString(1) + EndDate[12].toString(1)) + 1;
    var EndMinute = parseInt(EndDate[14].toString(1) + EndDate[15].toString(1));

    var EndTimeConvert = new Date(EndYear, EndMonth, EndDay, EndHour, EndMinute);
    
    var CurrentTime = new Date();

    if (CreatedFor == "1") {
        if (CurrentTime.getFullYear() == EndTimeConvert.getFullYear() && CurrentTime.getMonth() == EndTimeConvert.getMonth() && CurrentTime.getDay() == EndTimeConvert.getDay() && CurrentTime.getHours() == EndTimeConvert.getHours() && CurrentTime.getMinutes() == EndTimeConvert.getMinutes()) {
            var ReturnObject = new Object();
            ReturnObject.ReturnValue = true;
            ReturnObject.ReturnTextValue = "Finishing";
            ReturnObject.ReturnTimeValue = 0;
            return ReturnObject;
        }
        if (CurrentTime > EndTimeConvert) {
            const diffTime = Math.abs(CurrentTime - EndTimeConvert);
            const diffseconds = Math.ceil(diffTime / (1000));

            if (diffseconds < 60) {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Finishing";
                ReturnObject.ReturnTimeValue = 0;
                return ReturnObject;
            }
            else {
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Finished";
                ReturnObject.ReturnTimeValue = diffseconds;
                return ReturnObject;
            }
        }

        var ReturnObject = new Object();
        ReturnObject.ReturnValue = false;
        ReturnObject.ReturnTextValue = "NoFinish!";
        ReturnObject.ReturnTimeValue = 0;
        return ReturnObject;
    }
    else {
        var numberofdays = 0;
        var CurrentTime = new Date();
        var ReturnedObj = GetActiveDayAndNumberOfDays(StartDate, EndDate, Days, AcceptedNotification, "Finish", Stats);
        
        numberofdays = ReturnedObj.NumberOfDays;
        IsLowerDay = ReturnedObj.IsLower;
        IsActiveBefore = ReturnedObj.IsActiveBefore;
        
        var CurrentTime1 = new Date();

        if (EndHour < 24) {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays);
        }
        else {
            CurrentTime1.setDate(CurrentTime1.getDate() + numberofdays - 1);
        }
        

        
        CurrentTime1.setHours(EndHour);
        CurrentTime1.setMinutes(EndMinute);

        
        var DayInMonth = CurrentTime1.getDate();
        
        
        var RealCurrentTime = new Date();

        
            if ((RealCurrentTime.getFullYear() == CurrentTime1.getFullYear()) && (RealCurrentTime.getMonth() == CurrentTime1.getMonth()) && (RealCurrentTime.getDate() == DayInMonth) && (RealCurrentTime.getHours() == CurrentTime1.getHours()) && (RealCurrentTime.getMinutes() == CurrentTime1.getMinutes())) {
           
                var ReturnObject = new Object();
                ReturnObject.ReturnValue = true;
                ReturnObject.ReturnTextValue = "Finishing";
                ReturnObject.ReturnTimeValue = 0;
                return ReturnObject;
            }
            if (RealCurrentTime > CurrentTime1) {

                const diffTime = Math.abs(RealCurrentTime - CurrentTime1);
                const diffseconds = Math.ceil(diffTime / (1000));
            
                if (diffseconds < 60) {
                    var ReturnObject = new Object();
                    ReturnObject.ReturnValue = true;
                    ReturnObject.ReturnTextValue = "Finishing";
                    ReturnObject.ReturnTimeValue = 0;
                    return ReturnObject;
                }
                else {
                    if (Stats != "In processing!") {
                        var ReturnObject = new Object();
                        ReturnObject.ReturnValue = true;
                        ReturnObject.ReturnTextValue = "Finished";
                        ReturnObject.ReturnTimeValue = diffseconds;
                        return ReturnObject;
                    }
                }
            }
        

        var ReturnObject = new Object();
        ReturnObject.ReturnValue = false;
        ReturnObject.ReturnTextValue = "No!";
        ReturnObject.ReturnTimeValue = 0;
        return ReturnObject;
    }
}
function ClearAllTimeOutExceptLast(Index, HighBool) {
    if (HighBool == true) {
        for (var i = 0; i < TaskInterval.length; i++) {
            clearTimeout(TaskInterval[i]);
        }
    }
    else {
        for (var i = 0; i < TaskInterval.length; i++) {
            if (i != Index) {
                clearTimeout(TaskInterval[i]);
            }
        }
    }
}
$(document).ready(function (params) {
    MainDisplayNotification();
});
setInterval(MainDisplayNotification, 5000);