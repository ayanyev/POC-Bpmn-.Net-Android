<template>
  <div>
    <ArticlesList/>
  </div>
</template>

<script>

import Vue from "vue";
import VueSignalR from "@latelier/vue-signalr";
import ArticlesList from "@/components/ArticlesList";
// http://10.0.11.185:5000, http://192.168.0.38:5000
Vue.use(VueSignalR, 'http://10.0.11.185:5000/intakedashboardhub')

export default {
  name: "IntakeApp",
  components: {
    ArticlesList
  },
  created() {
    this.$socket.start({
      log: true
    });
  }
}
</script>

<style scoped>

</style>