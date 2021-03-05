import Vue from "vue";

const tasksList = new Vue({
    el: '#tasksList',
    data: {
        header: 'Available tasks:',
        items: []
    }
});
const createButton = new Vue({
    el: '#createButton',
    data: {
        label: 'New task',
        socket: null
    },
    methods: {
        onClick: function () {
            this.socket
                ? this.socket.send("Tasks request:create")
                : alert('socket is null')
        }
    }

})