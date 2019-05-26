<template>
  <v-container fluid>

    <MyProjects />

    <MyProjectInvitations />

    <JoinProject />

    <!--//* Create Project -->
    <v-flex xs12>
      <h2 class="headline mb-3">Create a new Project</h2>
    </v-flex>
    <v-flex
      xs12 sm10 md8 lg6
      class="mb-5"
    >
      <p>
        Are you the Coordinator of an event/a project?
        <br>
        Then you can create a new Project and invite all the Publishers to it.
      </p>
      <v-btn
        :to="{ name: 'createProject' }"
        color="warning">
        Create a new Project
      </v-btn>
    </v-flex>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import MyProjects from '@/components/MyProjects.vue'
import MyProjectInvitations from '@/components/MyProjectInvitations.vue'
import JoinProject from '@/components/JoinProject.vue'
import Component from 'vue-class-component'

@Component({
  components: {
    MyProjects,
    MyProjectInvitations,
    JoinProject
  }
})
export default class ConfigureProjects extends Vue {}
</script>
