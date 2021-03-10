<template>
  <div id="tasksList">
    <h3>{{ header }}</h3>
    <table>
      <thead>
      <tr>
        <td>#</td>
        <td>Guid</td>
        <td>Status</td>
      </tr>
      </thead>
      <tr v-for="(task, key) in tasks" :key="key">
        <td>{{ key + 1 }}</td>
        <td>{{ task.guid }}</td>
        <td>{{ task.status }}</td>
      </tr>
      <tfoot>
      <tr><td id="button-cell" colspan="3">
        <button v-on:click="onClick">{{ label }}</button>
      </td></tr>
      </tfoot>
    </table>
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
      this.tasks = data;
    }
  }
}
</script>

<style scoped>
#button-cell {
  text-align: center;
}
</style>