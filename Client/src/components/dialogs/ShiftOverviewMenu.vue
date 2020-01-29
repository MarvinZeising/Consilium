<template>

  <v-menu
    v-if="getShift"
    v-model="model.model"
    :close-on-content-click="false"
    :activator="model.element"
    offset-x
    offset-overflow
  >
    <ShiftOverviewMenuDraft
      v-if="getShift.status === 'draft'"
      :shift="getShift"
    />
    <v-card
      v-else
      min-width="400px"
      max-width="500px"
    >
      <v-toolbar
        :color="getEventColor"
        dark
        flat
      >
        <v-spacer></v-spacer>
        <ShiftAssignmentDialog
          v-if="canEdit && !getShift.isDraft"
          :shift="getShift"
          @saved="model.model = true"
        />
        <UpdateShiftDialog
          v-if="canEdit && getShift.isDraft"
          :shift="getShift"
        />
        <v-btn
          icon
          :title="$t('core.close')"
          @click="model.model = false"
        >
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>

      <v-toolbar
        :color="getEventColor"
        dark
        flat
      >
        <v-toolbar-title>{{ getShift.getTimespan(userModule.getUser) }}</v-toolbar-title>
        <v-spacer />
        <v-toolbar-title>{{ userModule.getUser.formatDate(getShift.date) }}</v-toolbar-title>
      </v-toolbar>

      <v-card-actions
        v-if="myApplication && !myAttendee"
        class="navbar"
      >
        <span
          v-if="getShift.status === 'planned'"
          class="ml-2"
          v-t="'shift.application.applied'"
        />
        <span
          v-if="getShift.status === 'scheduled'"
          class="ml-2"
          v-t="'shift.application.appliedBackup'"
        />
        <v-spacer />
        <DeleteApplicationDialog :application="myApplication" />
      </v-card-actions>

      <v-card-actions
        v-else-if="getShift.status === 'scheduled' && myAttendee"
        class="green"
      >
        <span
          class="ml-2"
          v-t="'shift.attendee.attending'"
        />
        <v-spacer />
        <DeleteAttendeeDialog :attendee="myAttendee" />
      </v-card-actions>

      <v-card-actions v-if="!myApplication && !myAttendee">
        <v-spacer />
        <CreateApplicationDialog :shift="getShift" />
      </v-card-actions>

      <v-list-group
        v-if="getShift.attendees.length > 0"
        :value="showAttendees"
        color="/*none*/"
      >
        <template v-slot:activator>
          <v-list-item-title>
            {{ $tc('shift.attendee.attendees', 2) }}
          </v-list-item-title>
        </template>
        <v-list-item
          v-for="(attendee, index) in getShift.getAttendees"
          :key="index"
          @click="openPerson()"
        >
          <v-list-item-content>
            <v-list-item-title>
              {{ attendee.person.getFullName }}
              <v-icon small v-if="attendee.isCaptain">flag</v-icon>
            </v-list-item-title>
            <v-list-item-subtitle>{{ attendee.person.congregation.name }}</v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-icon>info</v-icon>
          </v-list-item-action>
        </v-list-item>
      </v-list-group>

      <v-list-group
        v-if="getShift.applications.length > 0"
        :value="showApplicants"
        color="/*none*/"
      >
        <template v-slot:activator>
          <v-list-item-title>
            {{ $tc('shift.application.applicants', 2) }}
          </v-list-item-title>
        </template>
        <v-list-item
          v-for="(application, index) in getShift.getApplications"
          :key="index"
          @click="openPerson()"
        >
          <v-list-item-avatar>
            <v-icon color="accent">emoji_people</v-icon>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ application.person.getFullName }}</v-list-item-title>
            <v-list-item-subtitle>6 applications, 3 approved</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list-group>

    </v-card>
  </v-menu>

</template>

<style lang="scss" scoped>
  .toolbar-draft {
    background: repeating-linear-gradient(
      -45deg,
      #222,
      #222 10px,
      var(--v-accent-darken1) 10px,
      var(--v-accent-darken1) 20px,
    );
    text-shadow: 2px 2px #222;
  }
</style>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import UpdateShiftDialog from './UpdateShiftDialog.vue'
import HandlePlanShiftDialog from './HandlePlanShiftDialog.vue'
import ShiftOverviewMenuDraft from './ShiftOverviewMenuDraft.vue'
import ShiftAssignmentDialog from './ShiftAssignmentDialog.vue'
import CreateApplicationDialog from './CreateApplicationDialog.vue'
import DeleteApplicationDialog from './DeleteApplicationDialog.vue'
import DeleteAttendeeDialog from './DeleteAttendeeDialog.vue'
import { Shift, ShiftStatus } from '../../models'

@Component({
  components: {
    UpdateShiftDialog,
    HandlePlanShiftDialog,
    ShiftOverviewMenuDraft,
    ShiftAssignmentDialog,
    CreateApplicationDialog,
    DeleteApplicationDialog,
    DeleteAttendeeDialog,
  },
})
export default class CreateTeamDialog extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  @Prop(Object)
  private readonly model?: {
    model: any,
    element: any,
    event: any,
  }

  private showApplicants = true
  private showAttendees = true

  private get canEdit() {
    const role = this.personModule.getActiveRole
    if (role && role.calendarWrite) {
      const eligibility = role.eligibilities.find((x) => x.categoryId === this.getShift?.categoryId)
      if (eligibility) {
        return eligibility.shiftsWrite
      }
    }
    return false
  }

  private get getShift(): Shift | undefined {
    return this.model?.event.shift
  }

  private get getEventColor() {
     if (this.getShift?.status === ShiftStatus.scheduled) {
       return 'green'
     }
     return 'navbar'
  }

  private get myApplication() {
    const personId = this.personModule.getActivePersonId
    return this.getShift?.applications.find((x) => x.personId === personId)
  }

  private get myAttendee() {
    const personId = this.personModule.getActivePersonId
    return this.getShift?.attendees.find((x) => x.personId === personId)
  }

  @Watch('model.event')
  private async onModelChanged(val: any, oldVal: any) {
    setTimeout(() => {
      this.showApplicants = val?.shift?.attendees.length === 0
    }, 10)
  }

  private openPerson() {}

}
</script>
