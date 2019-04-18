<template>
  <v-container>
    <v-layout wrap>

      <!-- My Projects -->
      <v-flex xs12>
        <h2 class="headline mb-3">Your Projects</h2>
      </v-flex>

      <!-- Projects loading -->
      <v-flex
        v-if="loadingProjects"
        class="mb-5"
      >
        <v-progress-circular
          indeterminate
          color="primary"
        />
      </v-flex>

      <!-- No Projects existing -->
      <v-flex v-if="!loadingProjects && projects == []">
        <i>You don't have access to any Projects, yet.</i>
      </v-flex>

      <!-- Data -->
      <v-flex
        v-for="(project, i) in projects"
        :key="'project' + i"
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

      <!-- Project invitations -->
      <v-flex xs12>
        <h2 class="headline mb-3">Pending Project invitations</h2>
      </v-flex>

      <!-- Project invitations loading -->
      <v-flex
        v-if="loadingInvitations"
        class="mb-5"
      >
        <v-progress-circular
          indeterminate
          color="primary"
        />
      </v-flex>

      <!-- No Project invitations existing -->
      <v-flex v-if="!loadingInvitations && invitations == []">
        <i>You don't have any pending Project invitations.</i>
      </v-flex>

      <!-- Data -->
      <v-flex
        v-for="(project, i) in invitations"
        :key="'invitation' + i"
        xs12 sm8 md6 lg4
        class="mb-5 pa-2"
      >
        <v-card>
          <v-card-title primary-title>
            <h3 class="title mb-0 ml-2">Dummy Project</h3>
          </v-card-title>

          <v-card-actions>
            <v-btn
              flat
              color="primary"
            >
              Accept
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              flat
              color="error"
            >
              Decline
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

      <!-- Join Project -->
      <v-flex xs12>
        <h2 class="headline mb-3">Join an existing Project</h2>
      </v-flex>
      <v-flex
        xs12 sm8 md6 lg4
        class="mb-5"
      >
        <p>If you received a Project access token, you can enter the token here to join that Project:</p>
        <v-text-field
          v-model="accessToken"
          label="Project access token"
        ></v-text-field>
        <v-btn
          :disabled="accessToken == ''"
          color="primary"
          >
          Join this Project
        </v-btn>
      </v-flex>

      <!-- Create Project -->
      <v-flex xs12>
        <h2 class="headline mb-3">Create a new Project</h2>
      </v-flex>
      <v-flex
        xs12 sm8 md6 lg4
        class="mb-5"
      >
        <p>
          Are you the Coordinator of an event/a project?
          <br>
          Then you can create a new Project and invite all the Publishers to it.
        </p>
        <v-btn
          v-model="createProject"
          href="create-project"
          color="warning">
          Create a new Project
        </v-btn>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
export default Vue.extend({
  data () {
    return {
      loadingProjects: false,
      projects: [],
      loadingInvitations: false,
      invitations: [],
      accessToken: '',
      createProject: null
    }
  },
  created () {
    this.fetchProjects()
    this.fetchInvitations()
  },
  methods: {
    fetchProjects () {
      this.loadingProjects = true
      this.projects = []

      axios.get('/projects')
        .then((result) => {
          this.loadingProjects = false
          this.projects = result.data
        })
        .catch((err) => {
          this.loadingProjects = false
          console.error(err.toString())
        })
    },
    fetchInvitations () {
      this.loadingInvitations = true
      this.invitations = []

      axios.get('/projects')
        .then((result) => {
          const fetchedInvitations = result.data
          this.loadingInvitations = false
          this.invitations = fetchedInvitations.map((project : any) => {
            return {
              name: project.text
            }
          })
        })
        .catch((err) => {
          this.loadingInvitations = false
          console.error(err.toString())
        })
    }
  }
})
</script>
