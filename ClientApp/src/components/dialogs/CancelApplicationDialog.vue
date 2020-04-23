<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'core.cancel'" :loading="loading" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'shift.application.cancel'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'shift.application.cancelDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-spacer />
          <v-btn
            text
            type="submit"
            color="error"
            v-t="'shift.application.cancel'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="cancelApplication"
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
import { Shift } from '../../models'

@Component
export default class CancelApplicationDialog extends Vue {
  private applicationModule = getModule(ApplicationModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private valid = false
  private dialog = false
  private loading = false

  private async cancelApplication() {
    if (this.shift) {
      this.loading = true

      await this.applicationModule.cancelApplication(this.shift.id)

      this.loading = false
    }

    this.dialog = false
  }
}
</script>
