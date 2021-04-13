<template>
  <div>
    <PickersList></PickersList>
    <TaskList></TaskList>
  </div>
</template>

<script>

import PickersList from "./components/PickersList.vue";
import TaskList from "./components/TaskList.vue";
import Vue from "vue";
import VueSignalR from "@latelier/vue-signalr";

Vue.use(VueSignalR, 'http://localhost:5000/dashboardhub')

export default {
  name: 'PickerApp',
  components: {
    TaskList,
    PickersList
  },
  created() {
    this.$socket.start({
      log: true
    });
  }
}
</script>

<style>

</style>