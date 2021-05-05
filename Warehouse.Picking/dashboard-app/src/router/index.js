import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
    mode: "history",
    routes: [
        {
            path: '/intake',
            name: 'intake',
            component: () => import('../IntakeApp.vue')
        }
    ]
})