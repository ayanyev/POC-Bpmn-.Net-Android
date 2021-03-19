import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import App from './App.vue'
import VueSignalR from '@latelier/vue-signalr'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.config.productionTip = false
Vue.use(VueSignalR, 'http://localhost:5000/dashboardhub')
Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

new Vue({
  el: "#app",
  render: h => h(App),
  created() {
    this.$socket.start({
      log : true
    });
  },
})