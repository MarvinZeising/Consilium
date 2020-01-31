<template>
  <div v-if="shift">

    <div v-if="shift.isDraft" /><!-- // * nothing -->
    <div v-else-if="shift.isCalledOff">

      <v-card-actions class="grey" >
        <v-icon class="ml-2">error</v-icon>
        <span
          class="mx-2"
          v-t="'shift.status.calledOff'"
        />
      </v-card-actions>

    </div>
    <div v-else>

      <v-card-actions
        v-if="shift.isApplicant && !shift.isAttendee"
        :class="getColor"
      >
        <v-icon class="ml-2">emoji_people</v-icon>
        <span
          v-if="shift.isPlanned"
          class="mx-2"
          v-t="'shift.application.applied'"
        />
        <span
          v-if="!shift.isPlanned"
          class="mx-2"
          v-t="'shift.application.appliedBackup'"
        />
        <v-spacer />
        <CancelApplicationDialog :shift="shift" />
      </v-card-actions>

      <v-card-actions
        v-else-if="shift.isScheduled && shift.isAttendee"
        class="green"
      >
        <v-icon class="ml-2">check</v-icon>
        <span
          class="mx-2"
          v-t="'shift.attendee.attending'"
        />
        <v-spacer />
        <CancelAttendanceDialog :shift="shift" />
      </v-card-actions>

      <v-card-actions v-if="!shift.isApplicant && !shift.isAttendee">
        <v-spacer />
        <CreateApplicationDialog :shift="shift" />
      </v-card-actions>

    </div>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../../store/persons'
import CreateApplicationDialog from './CreateApplicationDialog.vue'
import CancelApplicationDialog from './CancelApplicationDialog.vue'
import CancelAttendanceDialog from './CancelAttendanceDialog.vue'
import { Shift, ShiftStatus } from '../../models'

@Component({
  components: {
    CreateApplicationDialog,
    CancelApplicationDialog,
    CancelAttendanceDialog,
  },
})
export default class ShiftOverviewMenuActions extends Vue {
  private personModule = getModule(PersonModule, this.$store)

  @Prop(Shift)
  private readonly shift?: Shift

  private get getColor() {
    if (this.shift?.isPlanned) {
      return 'navbar'
    } else if (this.shift?.isScheduled) {
      return 'green'
    } else if (this.shift?.isSuspended) {
      return 'red'
    } else if (this.shift?.isCalledOff) {
      return 'grey'
    }
  }

}
</script>
