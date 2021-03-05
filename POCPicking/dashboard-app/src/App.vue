<template>
  <div id="app">
    <PickersList></PickersList>
<!--    <div id="tasksList">-->
<!--      <h3>{{ header }}</h3>-->
<!--      <ol>-->
<!--        <li v-for="(item, key) in items" :key="key">{{ item }}</li>-->
<!--      </ol>-->
<!--    </div>-->
<!--    <div id="createButton">-->
<!--      <button v-on:click="onClick">{{ label }}</button>-->
<!--    </div>-->
  </div>
</template>

<script>

import PickersList from "./components/PickersList.vue";

export default {
  name: 'App',
  components: { 
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
