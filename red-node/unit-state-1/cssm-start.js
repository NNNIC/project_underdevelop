module.exports = function(RED) {
    function SetStartState(config) {
        RED.nodes.createNode(this,config);
        var node = this;

        this.on("input",function(msg) {
            this.send(msg);
            msg = null;
        });
        
    }
    RED.nodes.registerType("cssm-start",SetStartState);
}