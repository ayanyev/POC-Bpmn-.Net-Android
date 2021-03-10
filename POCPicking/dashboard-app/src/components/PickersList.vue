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
      <tr v-for="(name, key) in names" :key="key">
        <td>{{ key + 1 }}</td>
        <td>{{ name }}</td>
<!--        <td v-if="tasks[key] == null">-->
<!--          <button v-on:click="onClick(name)">assign task</button>-->
<!--        </td>-->
<!--        <td v-else-if>{{ tasks[key] }}</td>-->
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
      names: []
    }
  },
  mounted() {
    // this.$socket.invoke('GetAvailablePickers')
  },
  methods: {
    onClick: function () {
      // this.socket
      //     ? this.$socket.send('Tasks request:assign:' + name)
      //     : alert('socket is null')
    }
  },
  sockets: {
    AvailablePickers(data) {
      this.names = data.map(function(picker) {
        return picker.name;
      });
    }
  }
}
</script>

<style scoped>

</style>