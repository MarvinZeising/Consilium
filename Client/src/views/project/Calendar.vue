<template>
  <v-container
    v-if="canView"
    fluid
    class="pa-0"
  >

    <!--//* Toolbar -->
    <!--//TODO: On mobile, replace this toolbar with a simpler one (only day view and date picker) -->
    <v-toolbar flat>
      <v-btn-toggle
        dense
        class="mr-4"
      >
        <v-btn
          v-t="'shift.today'"
          @click="setToday"
        />
      </v-btn-toggle>

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.prev()"
      >
        <v-icon>keyboard_arrow_left</v-icon>
      </v-btn>

      <v-btn
        fab
        text
        small
        @click="$refs.calendar.next()"
      >
        <v-icon>keyboard_arrow_right</v-icon>
      </v-btn>

      <v-toolbar-title class="ml-4">{{ getTitle }}</v-toolbar-title>

      <v-spacer />

      <CreateShiftDialog
        :date="focus"
      />

      <v-btn-toggle
        v-model="type"
        color="primary"
        mandatory
        dense
      >
        <v-btn
          value="month"
          v-t="'shift.month'"
        />
        <v-btn
          value="week"
          v-t="'shift.week'"
        />
        <v-btn
          value="day"
          v-t="'shift.day'"
        />
      </v-btn-toggle>
    </v-toolbar>

    <!--//* Calendar -->
    <v-row>
      <v-flex
        xs12
      >
        <v-sheet>
          <v-calendar
            id="calendar"
            ref="calendar"
            color="primary"
            v-model="focus"
            :events="events"
            :type="type"
            :weekdays="weekdays"
            @click:event="showEvent"
            @click:more="viewDay"
          >
            <template v-slot:day-label="date">
              <v-menu
                :close-on-content-click="false"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    v-on="on"
                    fab
                    small
                    class="mb-1"
                    :icon="date.present ? false : true"
                    :color="date.present ? 'primary' : ''"
                    v-text="date.day"
                    rounded
                    depressed
                    @click="canEdit ? false : viewDay(date.date)"
                  />
                </template>
                <v-list>
                  <v-list-item @click="viewDay(date.date)">
                    <v-list-item-title v-t="'shift.goToDay'" />
                  </v-list-item>
                  <CreateShiftDialog
                    :date="date"
                    :menu="this"
                  />
                </v-list>
              </v-menu>

            </template>
          </v-calendar>
        </v-sheet>
      </v-flex>
    </v-row>

  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import i18n from '../../i18n'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import CreateShiftDialog from '../../components/dialogs/CreateShiftDialog.vue'

@Component({
  components: {
    CreateShiftDialog,
  }
})
export default class Calendar extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private type: string = 'month'
  private focus: string = moment().format('YYYY-MM-DD')
  private events: any = []
  private weekdays: number[] = [1, 2, 3, 4, 5, 6, 0]

  private start: any = null
  private end: any = null

  public get canView() {
    return this.personModule.getActiveRole?.calendarRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.calendarWrite === true
  }

  private get getTitle() {
    switch (this.type) {
      case 'month':
        return moment(this.focus).format('MMMM YYYY')
      case 'week':
        const week = i18n.t('shift.week')
        return moment(this.focus).format(`[${week}] w, MMM gggg`)
      case 'day':
        return this.userModule.getUser?.formatDate(this.focus)
    }
    return ''
  }

  private created() {
    this.events.push({
      name: 'Shift',
      start: moment().format('YYYY-MM-DD HH:mm'),
      end: moment().add(2, 'hours').format('YYYY-MM-DD HH:mm'),
      color: 'primary',
    })
  }

  private viewDay(date: any) {
    this.focus = date
    this.type = 'day'
  }

  private setToday() {
    this.focus = moment().format('YYYY-MM-DD')
  }

}
</script>
