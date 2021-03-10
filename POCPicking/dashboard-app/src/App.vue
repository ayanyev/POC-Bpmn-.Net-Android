<template>
  <div id="app">
    <PickersList></PickersList>
    <TaskList></TaskList>
  </div>
</template>

<script>

import PickersList from "./components/PickersList.vue";
import TaskList from "./components/TaskList.vue";

export default {
  name: 'App',
  components: {
    TaskList,
    PickersList
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>