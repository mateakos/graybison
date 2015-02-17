var sfctiport = 56802;
var jsonService = "http://localhost";

var com = {
    graybison: {
        salesforcecti: {
            client: {
                version: "v0.1",
                defaultTimeout: 15000,
                clientid : "",
                random: function (min, max) {
                    return Math.floor(Math.random() * (max - min) + min);
                },

                Heartbeat : function()
                {
                    //client identifier
                    var req = {
                        identifier : com.graybison.salesforcecti.client.clientid
                    };

                    var tm = com.graybison.salesforcecti.client.defaultTimeout;
                    if (req.hasOwnProperty("timeout")) {
                        tm = req.timeout;
                    }
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/HeartbeatService.svc/Heartbeat?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            com.graybison.salesforcecti.client.Heartbeat();
                        },
                        timeout: tm,
                        error: function (jqXHR, textStatus, errorThrown) {
                            $("#dconsole").html("Event queue error: " + textStatus + ", " + errorThrown);
                    }
                    });
                },

                LoginAgent: function (req, callback) {
                    if (callback === undefined) return;
                    var tm = com.graybison.salesforcecti.client.defaultTimeout;
                    if (req.hasOwnProperty("timeout")) {
                        tm = req.timeout;
                    }
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/LoginAgent?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: tm,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                LogoutAgent: function (req, callback) {
                    if (callback === undefined) return;
                    var tm = com.graybison.salesforcecti.client.defaultTimeout;
                    if (req.hasOwnProperty("timeout")) {
                        tm = req.timeout;
                    }
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/LogoutAgent?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: tm,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                SetAgentState: function (req, callback) {
                    if (callback === undefined) return;
                    var tm = com.graybison.salesforcecti.client.defaultTimeout;
                    if (req.hasOwnProperty("timeout")) {
                        tm = req.timeout;
                    }
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/SetAgentState?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: tm,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                }
            }
        }
    }
}
