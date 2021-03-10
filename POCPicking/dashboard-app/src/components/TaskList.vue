<template>
  <div id="tasksList">
    <h3>{{ header }}</h3>
    <ol>
      <li v-for="(task, key) in tasks" :key="key">{{ task }}</li>
    </ol>
    <div id="createButton">
      <button v-on:click="onClick">{{ label }}</button>
    </div>
  </div>
</template>

<script>
export default {
  name: "TaskList",
  data() {
    return {
      header: 'Current tasks:',
      label: 'Create task',
      tasks: []
    }
  },
  methods: {
    onClick: function () {
      this.$socket.invoke('CreateNewTask')
    }
  },
  sockets: {
    AvailableTasks(data) {
      this.tasks = data.map(function(task) {
        return task.guid;
      });
    }
  }
}
</script>

<style scoped>

</style>