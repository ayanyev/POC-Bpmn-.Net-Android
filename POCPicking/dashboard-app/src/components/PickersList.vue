<template>
  <div id="pickersList">
    <h3>{{ header }}</h3>
    <table>
      <thead>
      <tr>
        <td>#</td>
        <td>Name</td>
        <td>Task</td>
      </tr>
      </thead>
      <tr v-for="(picker, key) in pickers" :key="key">
        <td>{{ key + 1 }}</td>
        <td>{{ picker.name }}</td>
        <td v-if="picker.task != null">{{ picker.task.guid }}</td>
        <td v-else></td>
      </tr>
    </table>
  </div>
</template>

<script>
export default {
  name: "PickersList",
  data() {
    return {
      header: 'Available pickers:',
      pickers: []
    }
  },
  mounted() {
    this.$socket.invoke('GetAvailablePickers')
  },
  sockets: {
    AvailablePickers(data) {
      this.pickers = data;
    }
  }
}
</script>

<style scoped>

</style>