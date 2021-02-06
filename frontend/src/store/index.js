import { createStore } from 'vuex'
import axios from 'axios'

export default createStore({
  state: {
    saveFiles: null,
    saveFilesLoaded: false
  },
  mutations: {
    set_save_files_loading (state) {
      state.saveFilesLoaded = false
    },
    set_save_files (state, saveFiles) {
      state.saveFiles = saveFiles
      state.saveFilesLoaded = true
    }
  },
  actions: {
    loadSaveFiles ({ commit }) {
      commit('set_save_files_loading')

      axios
        .get(
          'https://localhost:5001/api/save-file/list',
          {
            responseType: 'json',
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json; charset=utf-8;'
            }
          }
        )
        .then(response => {
          commit('set_save_files', response.data)
        })
        .catch(error => {
          console.err(error)
        })
    }
  },
  modules: {
  }
})
