<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text color="error" v-t="'shift.status.unplan'" :loading="loading" />
    </template>
    <v-card>
      <v-card-title>
        <span class="headline" v-t="'shift.status.unplan'" />
      </v-card-title>
      <v-card-text>
        <p class="subtitle-1" v-t="'shift.status.unplanDescription'" />
      </v-card-text>
      <v-card-actions>
        <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
        <v-spacer />
        <v-btn
          text
          type="submit"
          color="error"
          v-t="'shift.status.unplan'"
          :loading="loading"
          @click.prevent="submit"
        />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ShiftModule from '../../store/shifts'
import { Shift, ShiftStatus } from '../../models'

@Component
export default class UpdateShiftToDraftDialog extends Vue {
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private dialog = false
  private loading = false

  private async submit() {
    if (this.shift) {
      this.loading = true

      await this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.draft,
      })

      this.loading = false
    }

    this.dialog = false
  }
}
</script>
