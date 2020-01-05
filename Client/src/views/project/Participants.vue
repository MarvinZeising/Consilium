<template>
  <v-container fluid>
    <v-layout wrap>

      <ParticipantsInvitations />

      <ParticipantsRequests />

      <ParticipantsList />

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../../store/projects'
import { Project } from '../../models/definitions'
import ParticipantsInvitations from '../../components/ParticipantsInvitations.vue'
import ParticipantsRequests from '../../components/ParticipantsRequests.vue'
import ParticipantsList from '../../components/ParticipantsList.vue'

@Component({
  components: {
    ParticipantsInvitations,
    ParticipantsRequests,
    ParticipantsList,
  }
})
export default class Participants extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
}
</script>
