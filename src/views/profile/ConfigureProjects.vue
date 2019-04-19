<template>
  <v-container fluid>

    <MyProjects />

    <MyProjectInvitations />

    <!--//* Join Project -->
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

    <!--//* Create Project -->
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
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import MyProjects from '@/components/MyProjects.vue'
import MyProjectInvitations from '@/components/MyProjectInvitations.vue'
export default Vue.extend({
  components: { MyProjects, MyProjectInvitations },
  data () {
    return {
      accessToken: '',
      createProject: null
    }
  },
  created () {
    this.fetchInvitations()
  },
  methods: {
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
