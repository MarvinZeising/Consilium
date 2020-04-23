<template>
  <v-dialog v-model="dialog" max-width="400px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text :loading="loading" v-t="'shift.status.planShift'" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'shift.status.planShift'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'shift.status.planShiftDescription'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.no'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.yes'"
            :loading="loading"
            :disabled="!valid"
            @click.prevent="planShift"
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
import ShiftModule from '../../store/shifts'
import { Shift } from '../../models'
import { ShiftStatus } from '../../models/shift'

@Component
export default class HandlePlanShiftDialog extends Vue {
  private shiftModule = getModule(ShiftModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private valid = false
  private dialog = false
  private loading = false

  private async planShift() {
    if (this.shift) {
      this.loading = true

      await this.shiftModule.updateShiftStatus({
        shiftId: this.shift.id,
        status: ShiftStatus.planned,
      })

      this.loading = false
    }

    this.dialog = false
  }
}
</script>
