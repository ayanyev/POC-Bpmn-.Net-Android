<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
export default {
  name: "App",
  mounted() {
    const url = window.location.href
    console.log('url : ' + url)
    const name = url.split('/').slice(-1)[0]
    console.log('appName : ' + name)
    if (name.length > 0) {
      this.$router.push({name: name})
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: start;
  color: #2c3e50;
  margin-top: 60px;
}

.table-div {
  padding-left: 25px;
  padding-right: 25px;
}

/*.table-bordered, td, th {*/
/*  border-collapse: collapse;*/
/*}*/
/*td, th, tfoot {*/
/*  padding: 10px;*/
/*  text-align: left;*/
/*}*/
</style>