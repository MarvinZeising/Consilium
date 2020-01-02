<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'project.invitations'"
    />
    <v-card flat class="ma-2 mb-5">
      <v-card-text
        class="grey--text"
        v-t="'project.invitationsDescription'"
      />
      <v-list>
        <v-list-item
          v-if="getInvitations.length === 0"
          dark
          class="warning"
        >
          <span v-t="'project.noInvitations'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in getInvitations"
          :key="index"
          three-line
        >
          <v-list-item-content>
            <v-list-item-title v-text="getPerson(participation.personId).fullName()" />
            <v-list-item-subtitle>Role: Administrator</v-list-item-subtitle>
            <v-list-item-subtitle>Invited on Dec 12, 2019</v-list-item-subtitle>
          </v-list-item-content>

          <v-list-item-action>
            <v-btn
              text
              class="mt-2"
              @click=""
              v-t="'core.edit'"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-t="'project.invitePerson'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../store/projects'
import { Person, ProjectParticipationStatus, Gender } from '../models/definitions'

@Component
export default class PersonsInvitations extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private get getInvitations() {
    const projectId = this.$route.params.projectId

    return this.projectModule.getParticipations
      .filter((p) => p.projectId === projectId && p.status === ProjectParticipationStatus.Invited)
  }

  private  getPerson(personId: string) {
    return this.projectModule.getParticipations.find((x) => x.id === personId)
  }

}
</script>
