<template>
  <div>
    <ArticlesList/>
  </div>
</template>

<script>

import Vue from "vue";
import VueSignalR from "@latelier/vue-signalr";
import ArticlesList from "@/components/ArticlesList";

Vue.use(VueSignalR, 'http://localhost:5000/intakedashboardhub')

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