const workersList = new Vue({
    el: '#workersList',
    mounted() {
        this.$socket.invoke("GetAvailablePickers")
    },
    data: {
        header: 'Available pickers:',
        names: [],
        tasks: []
    },
    methods: {
        onClick: function (name) {
            this.socket
                ? this.$socket.send('Tasks request:assign:' + name)
                : alert('socket is null')
        }
    },
    sockets: {
        availablePickers(data) {
            this.names = data.map(function(picker) {
                return picker.name;
            });
        }
    }
});
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