<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'core.details'"
      />
    </template>
    <v-card>
      <v-card-title>
        <span
          class="headline"
          v-t="'project.participation.details'"
        />
      </v-card-title>
      <v-card-text>
        <v-row class="ma-0">
          <span v-t="'project.participation.projectName'" />
          <v-spacer />
          <span v-text="participation.project.name" />
        </v-row>
        <v-row class="ma-0">
          <span v-t="'project.participation.assignedRole'" />
          <v-spacer />
          <span v-text="participation.role.name" />
        </v-row>
        <v-row class="ma-0">
          <span v-t="'project.participation.joinedOn'" />
          <v-spacer />
          <span v-text="userModule.getUser.formatDate(participation.createdTime)" />
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-btn
          text
          @click.stop="dialog = false"
          v-t="'core.close'"
        />
        <v-spacer />
        <DeleteParticipationDialog :participation="participation" />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import DeleteParticipationDialog from './DeleteParticipationDialog.vue'
import { Participation } from '../../models'

@Component({
  components: {
    DeleteParticipationDialog,
  }
})
export default class UpdateParticipationDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)

  @Prop(Participation)
  private readonly participation?: Participation

  private dialog: any = false

}
</script>
