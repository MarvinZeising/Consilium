<template>
  <div v-if="shift">

    <v-card-actions
      v-if="myApplication && !myAttendee"
      :class="shift.isPlanned ? 'navbar' : shift.isScheduled ? 'green' : ''"
    >
      <v-icon class="ml-2">emoji_people</v-icon>
      <span
        v-if="shift.isPlanned"
        class="ml-2"
        v-t="'shift.application.applied'"
      />
      <span
        v-if="shift.isScheduled"
        class="ml-2"
        v-t="'shift.application.appliedBackup'"
      />
      <v-spacer />
      <CancelApplicationDialog :shift="shift" />
    </v-card-actions>

    <v-card-actions
      v-else-if="shift.isScheduled && myAttendee"
      class="green"
    >
      <v-icon class="ml-2">check</v-icon>
      <span
        class="ml-2"
        v-t="'shift.attendee.attending'"
      />
      <v-spacer />
      <CancelAttendanceDialog :shift="shift" />
    </v-card-actions>

    <v-card-actions v-if="!myApplication && !myAttendee">
      <v-spacer />
      <CreateApplicationDialog :shift="shift" />
    </v-card-actions>

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

  private get myApplication() {
    const personId = this.personModule.getActivePersonId
    return this.shift?.applications.find((x) => x.personId === personId)
  }

  private get myAttendee() {
    const personId = this.personModule.getActivePersonId
    return this.shift?.attendees.find((x) => x.personId === personId)
  }

}
</script>
