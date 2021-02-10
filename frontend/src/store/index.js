import { createStore } from 'vuex'
import axios from 'axios'
import * as d3 from 'd3'

export default createStore({
  state: {
    saveFileList: null,
    saveFileListLoaded: false,
    saveFileHeaders: {},
    saveFileHeadersLoaded: {},
    saveFileObjects: {},
    saveFileObjectsLoaded: {},
    saveFileObjectsTree: {},
    saveFileObjectsTreeLoaded: {},
    saveFileProperties: {},
    saveFilePropertiesLoaded: {},
    saveFileDiff: {},
    saveFileDiffLoaded: {}
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
      state.saveFileHeaders[filename] = saveFileHeader
      state.saveFileHeadersLoaded[filename] = true
    },
    reset_save_file_objects (state, filename) {
      delete state.saveFileObjects[filename]
      state.saveFileObjectsLoaded[filename] = false

      delete state.saveFileObjectsTree[filename]
      state.saveFileObjectsTreeLoaded[filename] = false
    },
    set_save_file_objects (state, { filename, saveFileObjects }) {
      state.saveFileObjects[filename] = saveFileObjects
      state.saveFileObjectsLoaded[filename] = true
    },
    set_save_file_objects_tree (state, { filename, saveFileObjectsTree }) {
      console.log('set_save_file_objects_tree', arguments)
      state.saveFileObjectsTree[filename] = saveFileObjectsTree
      state.saveFileObjectsTreeLoaded[filename] = true
    },
    reset_save_file_properties (state, filename) {
      delete state.saveFileProperties[filename]
      state.saveFilePropertiesLoaded[filename] = false
    },
    set_save_file_properties (state, { filename, saveFileProperties }) {
      state.saveFileProperties[filename] = saveFileProperties
      state.saveFilePropertiesLoaded[filename] = true
    },
    reset_save_file_diff (state, { filename1, filename2 }) {
      if (Object.prototype.hasOwnProperty.call(state.saveFileDiff, filename2)) {
        delete state.saveFileDiff[filename2][filename1]
      }

      if (!Object.prototype.hasOwnProperty.call(state.saveFileDiff, filename2)) {
        state.saveFileDiffLoaded[filename2] = {}
      }
      state.saveFileDiffLoaded[filename2][filename1] = false
    },
    set_save_file_diff (state, { filename1, filename2, diff }) {
      if (!Object.prototype.hasOwnProperty.call(state.saveFileDiff, filename2)) {
        state.saveFileDiff[filename2] = {}
        state.saveFileDiffLoaded[filename2] = {}
      }
      state.saveFileDiff[filename2][filename1] = diff
      state.saveFileDiffLoaded[filename2][filename1] = true
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
    loadSaveFileObjects ({ commit, dispatch }, { filename }) {
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
          commit('set_save_file_objects', { filename, saveFileObjects: response.data })
          dispatch('buildSaveFileObjectsTree', { filename })
        })
        .catch(error => {
          console.error(error)
        })
    },
    loadSaveFileProperties ({ commit, dispatch }, { filename }) {
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
          dispatch('buildSaveFileObjectsTree', { filename })
        })
        .catch(error => {
          console.error(error)
        })
    },
    buildSaveFileObjectsTree ({ state, commit }, { filename }) {
      if (!Object.prototype.hasOwnProperty
          .call(state.saveFileObjects, filename) ||
        undefined === state.saveFileObjects[filename]
      ) {
        return
      }

      // index as a field
      const objects = state.saveFileObjects[filename].objects.reduce(
        (t, v, i) => {
          t.push({
            ...v,
            index: i
          })
          return t
        },
        []
      )

      const objectsByPathAndType = d3.rollup(
        objects,
        (v) => {
          return v.reduce((t, d) => {
            let data = {
              instanceName: d.id.instanceName,
              ...d.value
            }

            if (Object.prototype.hasOwnProperty
                .call(state.saveFileProperties, filename) &&
              undefined !== state.saveFileProperties[filename] &&
              Object.prototype.hasOwnProperty
                .call(state.saveFileProperties[filename].properties, d.index) &&
              undefined !== state.saveFileProperties[filename].properties[d.index]
            ) {
              const propsHierarchy = d3.index(
                state.saveFileProperties[filename].properties[d.index].properties,
                d => d.name + '[' + d.index + ']'
              )

              data = {
                ...data,
                properties: propsHierarchy
              }
            }

            return data
          }, [])
        },
        d => d.id.rootObject,
        d => d.value.type,
        d => d.typePath.split('/')[1],
        d => d.typePath.split('/')
          .reduce((a, v, i) =>
            i > 1
              ? a + (a.length > 0 ? '/' : '') + v
              : ''
          ),
        d => d.id.instanceName
      )

      commit('set_save_file_objects_tree', { filename, saveFileObjectsTree: objectsByPathAndType })
    },
    loadSaveFileDiff ({ commit }, { filename1, filename2 }) {
      commit('reset_save_file_diff', { filename1, filename2 })
      axios
        .get(
          'https://localhost:5001/api/save-file/' +
            encodeURIComponent(filename2) +
            '/diff/' +
            encodeURIComponent(filename1),
          {
            responseType: 'json',
            headers: {
              Accept: 'application/json',
              'Content-Type': 'application/json; charset=utf-8;'
            }
          }
        )
        .then(response => {
          commit('set_save_file_diff', { filename1, filename2, saveFileDiff: response.data.diff })
        })
        .catch(error => {
          console.error(error)
        })
    }
  },
  modules: {
  }
})
