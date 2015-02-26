var sfctiport = 56802;
var jsonService = "http://localhost";

var com = {
    graybison: {
        spare: {
            client: {
                version: "v0.1",
                defaultTimeout: 15000,
                queueTimeout: 30000,
                clientid : "",
                random: function (min, max) {
                    return Math.floor(Math.random() * (max - min) + min);
                },

                Heartbeat : function()
                {
                    var req = {
                        identifier: com.graybison.spare.client.clientid
                    }

                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/HeartbeatService.svc/Heartbeat?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            com.graybison.spare.client.clientid = data.Identifier;
                            com.graybison.spare.client.ProcessEvent(data);
                            com.graybison.spare.client.Heartbeat();
                        },
                        timeout: com.graybison.spare.client.queueTimeout,
                        error: function (jqXHR, textStatus, errorThrown) {
                            $("#dconsole").html("Event queue error: " + textStatus + ", " + errorThrown);
                    }
                    });
                },

                LoginAgent: function (req, callback) {
                    if (callback === undefined) return;
                    req.identifier = com.graybison.spare.client.clientid;
                    
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/LoginAgent?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: com.graybison.spare.client.queueTimeout,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                LogoutAgent: function (req, callback) {
                    if (callback === undefined) return;
                    req.identifier = com.graybison.spare.client.clientid;
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/LogoutAgent?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: com.graybison.spare.client.queueTimeout,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                SetAgentState: function (req, callback) {
                    if (callback === undefined) return;
                    req.identifier = com.graybison.spare.client.clientid;
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/AgentStateService.svc/SetAgentState?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: com.graybison.spare.client.queueTimeout,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                MonitorStation: function (req, callback) {
                    if (callback === undefined) return;
                    
                    req.identifier = com.graybison.spare.client.clientid;
                    var jqXHR = $.ajax({
                        url: jsonService + ":" + sfctiport + "/StationService.svc/CallObserve?jsoncallback=?",
                        datatype: 'jsonp',
                        data: req,
                        success: function (data) {
                            callback(data);
                        },
                        timeout: com.graybison.spare.client.defaultTimeout,
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback({ error: textStatus });
                        }
                    });
                },

                ProcessEvent: function (data) {
                    $("#dconsole").html(data);
                }
            }
        }
    }
}
