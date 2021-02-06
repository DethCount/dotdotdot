import { createStore } from 'vuex'
import axios from 'axios'

export default createStore({
  state: {
    saveFileList: null,
    saveFileListLoaded: false,
    saveFileHeaders: {},
    saveFileHeadersLoaded: {},
    saveFileObjects: {},
    saveFileObjectsLoaded: {},
    saveFileProperties: {},
    saveFilePropertiesLoaded: {}
  },
  mutations: {
    set_save_file_list_loading (state) {
      state.saveFileListLoaded = false
    },
    set_save_file_list (state, saveFileList) {
      state.saveFileList = saveFileList
      state.saveFileListLoaded = true
    },
    reset_save_file_header (state, filename) {
      delete state.saveFileHeaders[filename]
      state.saveFileHeadersLoaded[filename] = false
    },
    set_save_file_header (state, { filename, saveFileHeader }) {
      console.log('set_save_file_header', arguments)
      state.saveFileHeaders[filename] = saveFileHeader
      state.saveFileHeadersLoaded[filename] = true
    },
    reset_save_file_objects (state, filename) {
      delete state.saveFileObjects[filename]
      state.saveFileObjectsLoaded[filename] = false
    },
    set_save_file_objects (state, { filename, saveFileObjects }) {
      console.log('set_save_file_objects', arguments)
      state.saveFileObjects[filename] = saveFileObjects
      state.saveFileObjectsLoaded[filename] = true
    },
    reset_save_file_properties (state, filename) {
      delete state.saveFileProperties[filename]
      state.saveFilePropertiesLoaded[filename] = false
    },
    set_save_file_properties (state, { filename, saveFileProperties }) {
      console.log('set_save_file_properties', arguments)
      state.saveFileProperties[filename] = saveFileProperties
      state.saveFilePropertiesLoaded[filename] = true
    }
  },
  actions: {
    loadSaveFileList ({ commit }) {
      commit('set_save_file_list_loading')

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
          commit('set_save_file_list', response.data)
        })
        .catch(error => {
          console.error(error)
        })
    },
    loadSaveFileHeader ({ commit }, { filename }) {
      console.log(arguments)
      commit('reset_save_file_header', filename)

      axios
        .get(
          'https://localhost:5001/api/save-file/' +
            encodeURIComponent(filename) +
            '/header',
          {
            responseType: 'json',
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json; charset=utf-8;'
            }
          }
        )
        .then(response => {
          commit('set_save_file_header', { filename: filename, saveFileHeader: response.data })
        })
        .catch(error => {
          console.error(error)
        })
    },
    loadSaveFileObjects ({ commit }, { filename }) {
      console.log(arguments)
      commit('reset_save_file_objects', filename)

      axios
        .get(
          'https://localhost:5001/api/save-file/' +
            encodeURIComponent(filename) +
            '/objects',
          {
            responseType: 'json',
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json; charset=utf-8;'
            }
          }
        )
        .then(response => {
          commit('set_save_file_objects', { filename: filename, saveFileObjects: response.data })
        })
        .catch(error => {
          console.error(error)
        })
    },
    loadSaveFileProperties ({ commit }, { filename }) {
      console.log(arguments)
      commit('reset_save_file_properties', filename)

      axios
        .get(
          'https://localhost:5001/api/save-file/' +
            encodeURIComponent(filename) +
            '/properties',
          {
            responseType: 'json',
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json; charset=utf-8;'
            }
          }
        )
        .then(response => {
          commit('set_save_file_properties', { filename: filename, saveFileProperties: response.data })
        })
        .catch(error => {
          console.error(error)
        })
    }
  },
  modules: {
  }
})
