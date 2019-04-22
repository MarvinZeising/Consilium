<template>
  <div class="myProjects">

    <!--//* My Projects -->
    <v-flex xs12>
      <h2 class="headline mb-3">Your Projects</h2>
    </v-flex>

    <!--//* Projects loading -->
    <v-flex
      v-if="loading"
      class="mb-5"
    >
      <v-progress-circular
        indeterminate
        color="primary"
      />
    </v-flex>

    <!--//* No Projects existing -->
    <v-flex v-if="!loading && projects == []">
      <i>You don't have access to any Projects, yet.</i>
    </v-flex>

    <!--//* Data -->
    <v-flex
      v-for="(project, i) in projects"
      :key="i"
      xs12 sm8 md6 lg4
      class="mb-5 pa-2"
    >
      <v-card>
        <v-card-title primary-title>
          <h3 class="title mb-0 ml-2">
            {{ project.name }}
          </h3>
        </v-card-title>

        <v-card-actions>
          <v-btn
            flat
            color="primary"
          >
            Show permissions
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            flat
            color="error"
          >
            Leave Project
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-flex>

  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
export default Vue.extend({
  name: 'MyProjects',
  data () {
    return {
      loading: false,
      projects: []
    }
  },
  created () {
    this.fetchProjects()
  },
  methods: {
    fetchProjects () {
      this.loading = true
      this.projects = []

      axios.get('/projects')
        .then((result) => {
          this.loading = false
          this.projects = result.data
        })
        .catch((err) => {
          this.loading = false
          console.error(err.toString())
        })
    },
  }
})
</script>
