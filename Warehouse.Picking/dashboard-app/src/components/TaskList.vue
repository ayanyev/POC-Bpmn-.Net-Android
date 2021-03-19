<template>
  <b-container id="tasksList">
    <h4>{{ header }}</h4>
    <b-table bordered hover :fields="fields" :items="tasks" head-variant="dark"></b-table>
    <b-button squared variant="dark" v-on:click="onClick">{{ label }}</b-button>
  </b-container>
</template>

<script>
export default {
  name: "TaskList",
  data() {
    return {
      header: 'Current tasks:',
      label: 'Create task',
      fields: [
        {key: 'index', label: '#'},
        {key: 'guid',},
        {key: 'status'},
      ],
      tasks: []
    }
  },
  mounted() {
    this.$socket.invoke('GetAvailableTasks')
  },
  methods: {
    onClick: function () {
      this.$socket.invoke('CreateNewTask')
    }
  },
  sockets: {
    AvailableTasks(data) {
      let d = []
      for (let i = 0; i < data.length; i++) {
        d[i] = {index: i + 1, guid: data[i].guid, status: data[i].status}
      }
      this.tasks = d;
    }
  }
}
</script>

<style scoped>
#button-cell {
  text-align: center;
}
</style>