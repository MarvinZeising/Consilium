<template>
  <v-container fluid>

    <MyProjects />

    <MyProjectInvitations />

    <JoinProject />

    <!--//* Create Project -->
    <v-flex xs12>
      <h2
        class="headline mb-3"
        v-t="'project.create'"
      />
    </v-flex>
    <v-flex
      xs12 sm10 md8 lg6
      class="mb-5"
    >
      <p>
        {{ $t('project.createDescription1') }}
        <br>
        {{ $t('project.createDescription2') }}
      </p>
      <v-btn
        :to="{ name: 'createProject' }"
        color="warning"
        v-t="'project.create'"
      />
    </v-flex>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import MyProjects from '../../components/MyProjects.vue'
import MyProjectInvitations from '../../components/MyProjectInvitations.vue'
import JoinProject from '../../components/JoinProject.vue'
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
