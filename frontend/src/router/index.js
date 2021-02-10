import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'

const routes = [
  {
    path: '/file/:filename',
    name: 'SaveFileRead',
    props: true,
    component: () => import('../views/SaveFile/Read.vue')
  },
  {
    path: '/file/:filename2/diff/:filename1',
    name: 'SaveFileDiff',
    props: true,
    component: () => import('../views/SaveFile/ViewDiff.vue')
  },
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import('../views/About.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
