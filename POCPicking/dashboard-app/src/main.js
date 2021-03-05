import Vue from 'vue'
import App from './App.vue'
import VueSignalR from '@latelier/vue-signalr'

Vue.config.productionTip = false
Vue.use(VueSignalR, 'https://localhost:5001/dashboardhub')

new Vue({
  el: "#app",
  render: h => h(App),
  created() {
    this.$socket.start({
      log : true
    });
  },
})