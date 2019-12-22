<template>
  <v-container fluid>

    <PersonalProjects />

    <PersonalProjectInvitations />

    <JoinProject />

    <!--//* Create Project -->
    <v-flex xs12>
      <h2
        class="headline mb-3"
        v-t="'project.create'"
      />
    </v-flex>
    <v-flex
      xs12 sm10 md8 lg6 xl4
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
import { Vue, Component } from 'vue-property-decorator'
import PersonalProjects from '../../components/PersonalProjects.vue'
import PersonalProjectInvitations from '../../components/PersonalProjectInvitations.vue'
import JoinProject from '../../components/JoinProject.vue'

@Component({
  components: {
    PersonalProjects,
    PersonalProjectInvitations,
    JoinProject
  }
})
export default class ConfigureProjects extends Vue {}
</script>
