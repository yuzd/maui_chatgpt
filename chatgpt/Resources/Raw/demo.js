
// 调用csharp的方法
class CsharpMethod {
    constructor(command, data) {
        this.RequestPrefix = "request_csharp_";
        this.ResponsePrefix = "response_csharp_";
        // 唯一
        this.dataId = this.RequestPrefix + new Date().getTime();
        // 调用csharp的命令
        this.command = command;
        // 参数
        this.data = { command: command, data: !data ? '' : JSON.stringify(data), key: this.dataId }
    }

    // 调用csharp 返回promise
    call() {
        // 把data存储到localstorage中 目的是让csharp端获取参数
        localStorage.setItem(this.dataId, this.utf8_to_b64(JSON.stringify(this.data)));
        let eventKey = this.dataId.replace(this.RequestPrefix, this.ResponsePrefix);
        let that = this;
        var promise = new Promise(function (resolve, reject) {
            var eventHandler = function (e) {
                debugger;
                window.removeEventListener(eventKey, eventHandler);
                let resp = e.newValue;
                if (resp) {
                    // 从base64转换
                    let realData = that.b64_to_utf8(resp);
                    if (realData.startsWith('err:')) {
                        reject(realData.substr(4));
                    } else {
                        try {
                            let rspData = JSON.parse(realData)
                            resolve(rspData);
                        } catch (e) {
                            resolve(realData);
                        }
                    }
                } else {
                    reject("unknow error ： " + eventKey);
                }
            }
            // 注册监听回调
            window.addEventListener(eventKey, eventHandler);
        });
        // 改变location 发送给csharp端
        window.location = "/api/" + this.dataId;
        return promise;
    }

    // 转成base64 解决中文乱码
    utf8_to_b64(str) {
        return window.btoa(unescape(encodeURIComponent(str)));
    }
    // 从base64转过来 解决中文乱码
    b64_to_utf8(str) {
        return decodeURIComponent(escape(window.atob(str)));
    }

}