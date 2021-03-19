<template>
  <b-container id="pickersList">
    <h4>{{ header }}</h4>
    <b-table bordered hover :fields="fields" :items="pickers" head-variant="dark"></b-table>
  </b-container>
</template>

<script>
export default {
  name: "PickersList",
  data() {
    return {
      header: 'Available pickers:',
      fields: [
        {key: 'index', label: '#'},
        {key: 'name', label: 'Name'},
        {key: 'guid', label: 'Task'},
      ],
      pickers: []
    }
  },
  mounted() {
    this.$socket.invoke('GetAvailablePickers')
  },
  sockets: {
    AvailablePickers(data) {
      let d = []
      for (let i = 0; i < data.length; i++) {
        d[i] = {index: i + 1, name: data[i].name, guid: data[i].task?.guid ?? 'n/a'}
      }
      this.pickers = d
    }
  }
}
</script>

<style scoped>

</style>