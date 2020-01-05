<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        color="error"
        v-t="'core.delete'"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.invitation.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'project.invitation.deleteDescription'"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            :loading="loading"
            type="submit"
            text
            color="error"
            @click.stop="deleteInvitation"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'

@Component
export default class DeleteInvitationDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  @Prop(String)
  private readonly participationId?: string

  private valid: any = false
  private dialog: boolean = false
  private loading: boolean = false

  private async deleteInvitation() {
    if (this.participationId) {
      this.loading = true

      await this.projectModule.cancelInvitation(this.participationId)
    }

    this.dialog = false
  }

}
</script>
