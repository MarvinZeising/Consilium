<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'core.cancel'"
        :loading="loading"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'shift.application.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'shift.application.deleteDescription'"
          />
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-spacer />
          <v-btn
            text
            type="submit"
            color="error"
            v-t="'shift.application.delete'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="deleteApplication"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ApplicationModule from '../../store/applications'
import { Application } from '../../models'

@Component
export default class DeleteApplicationDialog extends Vue {
  private applicationModule = getModule(ApplicationModule, this.$store)

  @Prop(Application)
  private readonly application?: Application

  private valid: any = false
  private dialog: boolean = false
  private loading: boolean = false

  private async deleteApplication() {
    if (this.application) {
      this.loading = true

      await this.applicationModule.deleteApplication(this.application)

      this.loading = false
    }

    this.dialog = false
  }

}
</script>
