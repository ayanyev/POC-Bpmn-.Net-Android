<template>
  <div id="app">
    <div id="workersList">
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
          <td v-if="tasks[key] == null">
            <button v-on:click="onClick(name)">assign task</button>
          </td>
          <td v-else>{{ tasks[key] }}</td>
        </tr>
      </table>
    </div>
    <div id="tasksList">
      <h3>{{ header }}</h3>
      <ol>
        <li v-for="item in items">{{ item }}</li>
      </ol>
    </div>
    <div id="createButton">
      <button v-on:click="onClick">{{ label }}</button>
    </div>
  </div>
</template>

<script>

export default {
  name: 'App',
  components: {},
  mounted() {
    const plugin = document.createElement("script");
    plugin.setAttribute(
        "src",
        "./src/info.js"
    );
    plugin.async = true;
    document.head.appendChild(plugin);
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
