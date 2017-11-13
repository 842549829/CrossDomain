(function (window) {
    //指定路径
    window.setCookie = function (name, value, hour, path) {
        var cookie = name + "=" + encodeURIComponent(value) + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        if (path) {
            cookie += "path=" + path;
        }
        document.cookie = cookie;
    };

    /*当前路径写cookie*/
    window.setCookieCurrentPath = function (name, value, hour) {
        window.setCookie(name, value, hour);
    };
    /*当前路径按组写cookie*/
    window.setCookiesCurrentPath = function (name, value, hour) {
        var cookie = name + "=" + encodeURIComponent(name + "=" + value)
            + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
        document.cookie = cookie;
    };
    //读取cookie
    window.getCookie = function (name) {
        var re = new RegExp("(^|;)\\s*(" + name + ")=([^;]*)(;|$)", "i");
        var res = re.exec(document.cookie);
        return res != null ? decodeURIComponent(res[3]) : "";
    };
    /*按组读取cookie*/
    window.getCookies = function (name) {
        var result = {};
        var cookies = getCookie(name);
        var itemsCookies = cookies.split("&");
        for (var item in itemsCookies) {
            if (itemsCookies.hasOwnProperty(item)) {
                var items = itemsCookies[item].split("=");
                if (items.length < 2) continue;
                result[items[0]] = items[1];
            }
        }
        return result;
    };
})(window);
$(function () {
    $('#getData').click(function () {
        $.ajax({
            data: {
                IsSuccess: true,
                Data : "mydata"
            },
            type: "POST",
            url: 'http://localhost:18030/api/home/ListDataTest',
            headers: {
                // 授权字段
                Authorization: "aaa"
            },
            crossDomain: true,
            xhrFields: {
                //启用cookie
                withCredentials: true
            },
            success: function (data) {
                //以表格的形式在浏览器控制台显示数据,IE下不支持
                console.table(data);
            },
            error: function (e) {
                console.table(e);
            }
        });
    });
});